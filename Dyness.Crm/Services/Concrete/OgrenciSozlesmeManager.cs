using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Services.Helpers;
using Data.Abstract.Dapper;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class OgrenciSozlesmeManager : IOgrenciSozlesmeService
    {
        readonly IEfOgrenciSozlesmeData data;
        readonly IEfEtkinlikData etkinlikData;
        readonly IEfSubeData subeData;
        readonly IEfServisData servisData;
        readonly IEfHesapData hesapData;
        readonly IEfOgrenciSozlesmeTurData sozlesmeTurData;
        readonly IDpOgrenciSozlesmeData dpData;

        public OgrenciSozlesmeManager(
            IEfOgrenciSozlesmeData data,
            IEfEtkinlikData etkinlikData,
            IEfSubeData subeData,
            IEfServisData servisData,
            IEfHesapData hesapData,
            IEfOgrenciSozlesmeTurData sozlesmeTurData,
            IDpOgrenciSozlesmeData dpData)
        {
            this.data = data;
            this.etkinlikData = etkinlikData;
            this.subeData = subeData;
            this.servisData = servisData;
            this.hesapData = hesapData;
            this.sozlesmeTurData = sozlesmeTurData;
            this.dpData = dpData;
        }

        public EntityOperationResult<OgrenciSozlesme> Add(OgrenciSozlesme entity)
        {
            var ayarlar = AyarlarService.Get();
            var sube = subeData.GetByWhereCaseIncludeMultipleFirstOrDefault(x => x.SubeId == entity.SubeId, y => y.Hesap);

            var entityOperationResult = new EntityOperationResult<OgrenciSozlesme>(entity);

            var validationResults = EntityValidator<OgrenciSozlesme>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (sube == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SubeSecmekZorunlu
                });

                return entityOperationResult;
            }

            if (entity.SinifId == null && (entity.OgrenciSozlesmeTurId != 4 || entity.OgrenciSozlesmeTurId != 2))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SinifSecmekZorunlu
                });

                return entityOperationResult;
            }

            if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar != null && entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar > 0 && entity.OgrenciSozlesmeOdemeBilgi.PesinatHesapId == 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.PesinatHesapSecmekZorunlu
                });

                return entityOperationResult;
            }

            // Öğrenci kontrolleri
            var ogrenci = entity.Ogrenci;

            if (ogrenci != null)
            {
                // Eğer Öğrenci İletişim için seçildiyse, ceptelefon kontrol ediyoruz.
                if (ogrenci.IletisimKendi && string.IsNullOrEmpty(ogrenci.CepTelefon))
                {
                    //IletisimTercihEdlineCepTelefonBosOlamaz
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.OgrenciIletisimTercihEdlineCepTelefonBosOlamaz
                    });

                    return entityOperationResult;
                }
            }

            if (entity.OgrenciSozlesmeTurId != 2)
            {
                entity.YemekTutar = 0;
                entity.KiyafetTutar = 0;
            }

            if(entity.ServisId != null)
            {
                // Serviste yer var mı kontrol
                if (entity.ServisId != null && entity.ServisId != 0)
                {
                    var servis = servisData.GetById((int)entity.ServisId);

                    if (servis != null && servis.KapasiteKontrolEdilsinMi && !servis.KapasiteVarmi)
                    {
                        entityOperationResult.MessageInfos.Add(new MessageInfo
                        {
                            MessageInfoType = MessageInfoType.Error,
                            Message = Core.Properties.ServiceNoticesResources.ServisIcinYeterliKapasiteYok
                        });

                        return entityOperationResult;
                    }
                }
            }

            if (entity.OgrenciSozlesmeTurId == 3)
            {
                entity.YayinTutar = 0;
                entity.OzelDersDurumId = 1;
            }
            else
            {
                entity.OzelDersDurumId = null;
                entity.OgrenciSozlesmeDersSecimler = null;
            }

            if (entity.OgrenciSozlesmeTurId == 4)
            {
                //Etkinlik ise, etkinlik seçmek zorunlu olacak.
                if (entity.EtkinlikId == null || entity.EtkinlikId == 0)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.EtkinlikSecmekZorunlu
                    });

                    return entityOperationResult;
                }

                var etkinlik = etkinlikData.GetById((int)entity.EtkinlikId);

                if (etkinlik.KontenjanKontrolEdilsinMi && !etkinlik.KontenjanVarmi)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.EtkinlikIcinYeterliKontenjanYok
                    });

                    return entityOperationResult;
                }
            }

            if (entity.OgrenciSozlesmeTurId == 4 && entity.DanismanPersonelId == null)
            {
                //Etkinlik ise, danışman seçme zorunlu olacak.
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.EtkinlikIcinDanismanPersonelZorunlu
                });

                return entityOperationResult;
            }

            if (entity.OgrenciSozlesmeOdemeBilgi?.PesinatTutar != null && entity.OgrenciSozlesmeOdemeBilgi?.PesinatTutar > 0)
            {
                entity.ToplamOdenen = entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar;
            }

            // Tutar kontrol
            var checkToplamTutar = (entity.EgitimTutar ?? 0) +
                (entity.KiyafetTutar ?? 0) +
                (entity.ServisTutar ?? 0) +
                (entity.YayinTutar ?? 0) +
                (entity.YemekTutar ?? 0);

            if (entity.ToplamUcret != checkToplamTutar)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.ToplamTutarDogruDegil
                });

                return entityOperationResult;
            }

            // MinimumPeşinat oranı kontrol
            var pesinatOran = sube.MinimumPesinatOrani ?? ayarlar.MinimumPesinatOrani;
            var pesinatTutar = entity.ToplamUcret / 100 * (sube.MinimumPesinatOrani ?? ayarlar.MinimumPesinatOrani);

            if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar < pesinatTutar)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = $"{Core.Properties.ServiceNoticesResources.PesinatTutarYeterliDegil} {pesinatOran}"
                });

                return entityOperationResult;
            }

            var ogrenciEkle = entity.OgrenciId == 0;

            if (ogrenciEkle)
            {
                entity.Ogrenci.Hesap = new Hesap
                {
                    BagliKurumId = sube.KurumId,
                    ParaBirimId = (int)sube.ParaBirimId,
                    HesapBaslik = entity.Ogrenci.AdSoyad,
                    Ogrenci = entity.Ogrenci,
                    HesapTurId = 2,
                    EtkinMi = true,
                    HesapHareketler = new List<HesapHareket>(),
                };
            }
            else
            {
                entity.Ogrenci.Hesap = hesapData.GetById(entity.OgrenciId);
                entity.Ogrenci.Hesap.HesapHareketler = new List<HesapHareket>();
            }

            entity.EkleyenPersonelId = Identity.PersonelId;
            entity.Ogrenci.EkleyenPersonelId = Identity.PersonelId;
            entity.Ogrenci.SubeId = sube.SubeId;

            var ogrenciSozlesmeTur = sozlesmeTurData.GetById(entity.OgrenciSozlesmeTurId);

            var toplamTutarHareket = new HesapHareket
            {
                AlacakliHesapId = sube.Hesap.HesapId,
                ParaBirimId = (int)sube.ParaBirimId,
                HareketTarihi = DateTime.Now,
                IslemGerceklestiMi = true,
                Tutar = entity.ToplamUcret,
                Aciklama = $"{Core.Properties.ServiceNoticesResources.SozlesmeToplamTutar} ({ogrenciSozlesmeTur.OgrenciSozlesmeTurAd} - {entity.OlusturulmaTarihiFormatted})",
                PersonelId = Identity.PersonelId
            };

            toplamTutarHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
            {
                HesapHareket = toplamTutarHareket,
                OgrenciSozlesme = entity
            };

            if (ogrenciEkle)
                toplamTutarHareket.BorcluHesap = entity.Ogrenci.Hesap;
            else
                toplamTutarHareket.BorcluHesapId = entity.OgrenciId;

            entity.Ogrenci.Hesap.HesapHareketler.Add(toplamTutarHareket);

            decimal toplamTaksitTutar = 0;

            // Peşinat tutar kadar alacaklı
            if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar > 0)
            {
                entity.ToplamOdenen = entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar;

                var pesinatTutarHareket = new HesapHareket
                {
                    BorcluHesapId = entity.OgrenciSozlesmeOdemeBilgi.PesinatHesapId,
                    ParaBirimId = (int)sube.ParaBirimId,
                    HareketTarihi = DateTime.Now,
                    IslemGerceklestiMi = true,
                    Tutar = entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar,
                    Aciklama = Core.Properties.ServiceNoticesResources.PesinatAciklama,
                    PersonelId = Identity.PersonelId
                };

                pesinatTutarHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                {
                    HesapHareket = pesinatTutarHareket,
                    OgrenciSozlesme = entity
                };

                if (ogrenciEkle)
                    pesinatTutarHareket.AlacakliHesap = entity.Ogrenci.Hesap;
                else
                    pesinatTutarHareket.AlacakliHesapId = entity.OgrenciId;

                entity.Ogrenci.Hesap.HesapHareketler.Add(pesinatTutarHareket);

                toplamTaksitTutar += entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar ?? 0;
            }

            if (entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler != null && entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count > 0)
            {
                for (int i = 0; i < entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count; i++)
                {
                    var aylikTaksitBilgi = entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler[i];

                    if (aylikTaksitBilgi.TaksitTutari == null || aylikTaksitBilgi.TaksitTutari <= 0 || aylikTaksitBilgi.VadeTarihi == null)
                        continue;

                    var taksitHareket = new HesapHareket
                    {
                        BorcluHesapId = sube.Hesap.HesapId,
                        ParaBirimId = (int)sube.ParaBirimId,
                        VadeTarihi = (DateTime)aylikTaksitBilgi.VadeTarihi,
                        IslemGerceklestiMi = false,
                        Tutar = aylikTaksitBilgi.TaksitTutari,
                        Aciklama = $"{(i < 9 ? $"0{(i + 1).ToString()}" : (i + 1).ToString())}. {Core.Properties.ServiceNoticesResources.NoluTaksit}",
                        PersonelId = Identity.PersonelId
                    };

                    taksitHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                    {
                        HesapHareket = taksitHareket,
                        OgrenciSozlesme = entity
                    };

                    if (ogrenciEkle)
                        taksitHareket.AlacakliHesap = entity.Ogrenci.Hesap;
                    else
                        taksitHareket.AlacakliHesapId = entity.OgrenciId;

                    entity.Ogrenci.Hesap.HesapHareketler.Add(taksitHareket);

                    toplamTaksitTutar += aylikTaksitBilgi.TaksitTutari ?? 0;
                }
            }

            if (toplamTaksitTutar > 0 && Math.Abs((entity.ToplamUcret ?? 0) - toplamTaksitTutar) > (decimal)0.01 && entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler != null && entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.ToplamTutarIleTaksitliTutarEsitDegil
                });

                return entityOperationResult;
            }

            entity.OgrenciSozlesmeOdemeBilgi.ParaBirimId = (int)sube.ParaBirimId;

            if (entity.Ogrenci.AnneOgrenciYakiniIletisim != null && string.IsNullOrEmpty(entity.Ogrenci.AnneOgrenciYakiniIletisim.AdSoyad))
            {
                entity.Ogrenci.AnneOgrenciYakiniIletisim = null;
            }

            if (entity.Ogrenci.BabaOgrenciYakiniIletisim != null && string.IsNullOrEmpty(entity.Ogrenci.BabaOgrenciYakiniIletisim.AdSoyad))
            {
                entity.Ogrenci.BabaOgrenciYakiniIletisim = null;
            }

            if (entity.Ogrenci.YakiniOgrenciYakiniIletisim != null && string.IsNullOrEmpty(entity.Ogrenci.YakiniOgrenciYakiniIletisim.AdSoyad))
            {
                entity.Ogrenci.YakiniOgrenciYakiniIletisim = null;
            }

            if (entity.OlusturulmaTarihi == null)
                entity.OlusturulmaTarihi = DateTime.Now;

            if (entity.Ogrenci?.OlusturulmaTarihi == null)
                entity.Ogrenci.OlusturulmaTarihi = entity.OlusturulmaTarihi;

            var returnMessage = data.AddWithNestedLists(entity, sube.Hesap);

            if (!string.IsNullOrEmpty(returnMessage))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = returnMessage
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<OgrenciSozlesme> Update(OgrenciSozlesme entity)
        {
            var entityOperationResult = new EntityOperationResult<OgrenciSozlesme>(entity);

            var validationResults = EntityValidator<OgrenciSozlesme>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (entity.SinifId == null && (entity.OgrenciSozlesmeTurId != 4 || entity.OgrenciSozlesmeTurId != 2))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SinifSecmekZorunlu
                });

                return entityOperationResult;
            }

            var returnMessage = data.Update(entity);

            if (!string.IsNullOrEmpty(returnMessage))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = returnMessage
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<OgrenciSozlesme> UpdateOdemeBilgi(OgrenciSozlesme entity, bool odemelerSilinsinMi)
        {
            var ayarlar = AyarlarService.Get();
            var sube = subeData.GetByWhereCaseIncludeMultipleFirstOrDefault(x => x.SubeId == entity.SubeId, y => y.Hesap);

            var eskiModel = data.GetByWhereCaseIncludeMultiple(
                x => x.OgrenciSozlesmeId == entity.OgrenciSozlesmeId,
                y => y.Ogrenci,
                y => y.OgrenciSozlesmeHesapHareketler.Select(z => z.HesapHareket)).FirstOrDefault();

            var entityOperationResult = new EntityOperationResult<OgrenciSozlesme>(entity);

            var validationResults = EntityValidator<OgrenciSozlesme>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (sube == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SubeSecmekZorunlu
                });

                return entityOperationResult;
            }

            if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar != null && entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar > 0 && entity.OgrenciSozlesmeOdemeBilgi.PesinatHesapId == 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.PesinatHesapSecmekZorunlu
                });

                return entityOperationResult;
            }

            // Tutar kontrol
            var checkToplamTutar = (entity.EgitimTutar ?? 0) +
                (entity.KiyafetTutar ?? 0) +
                (entity.ServisTutar ?? 0) +
                (entity.YayinTutar ?? 0) +
                (entity.YemekTutar ?? 0);

            if (entity.ToplamUcret != checkToplamTutar)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.ToplamTutarDogruDegil
                });

                return entityOperationResult;
            }

            // MinimumPeşinat oranı kontrol
            var pesinatOran = sube.MinimumPesinatOrani ?? ayarlar.MinimumPesinatOrani;
            var pesinatTutar = entity.ToplamUcret / 100 * (sube.MinimumPesinatOrani ?? ayarlar.MinimumPesinatOrani);

            if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar < pesinatTutar)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = $"{Core.Properties.ServiceNoticesResources.PesinatTutarYeterliDegil} {pesinatOran}"
                });

                return entityOperationResult;
            }

            entity.OgrenciSozlesmeOdemeBilgi.SonGuncelleyenPersonelId = Identity.PersonelId;
            entity.OgrenciSozlesmeOdemeBilgi.SonGuncellenmeTarihi = DateTime.Now;

            var eklenecekHesapHareketler = new List<OgrenciSozlesmeHesapHareket>();
            var silinecekHesapHareketler = new List<OgrenciSozlesmeHesapHareket>();

            if (odemelerSilinsinMi)
            {
                var ogrenciSozlesmeTur = sozlesmeTurData.GetById(entity.OgrenciSozlesmeTurId);

                var toplamTutarHareket = new HesapHareket
                {
                    AlacakliHesapId = sube.Hesap.HesapId,
                    ParaBirimId = (int)sube.ParaBirimId,
                    HareketTarihi = DateTime.Now,
                    IslemGerceklestiMi = true,
                    Tutar = entity.ToplamUcret,
                    Aciklama = $"{Core.Properties.ServiceNoticesResources.SozlesmeToplamTutar} ({ogrenciSozlesmeTur.OgrenciSozlesmeTurAd} - {entity.OlusturulmaTarihiFormatted})",
                    PersonelId = Identity.PersonelId
                };

                toplamTutarHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                {
                    HesapHareket = toplamTutarHareket,
                    OgrenciSozlesmeId = entity.OgrenciSozlesmeId
                };

                toplamTutarHareket.BorcluHesapId = entity.OgrenciId;

                eklenecekHesapHareketler.Add(toplamTutarHareket.OgrenciSozlesmeHesapHareket);
            }

            if (odemelerSilinsinMi)
            {
                // Peşinat tutar kadar alacaklı
                if (entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar > 0)
                {
                    entity.ToplamOdenen = entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar;

                    var pesinatTutarHareket = new HesapHareket
                    {
                        BorcluHesapId = entity.OgrenciSozlesmeOdemeBilgi.PesinatHesapId,
                        ParaBirimId = (int)sube.ParaBirimId,
                        HareketTarihi = DateTime.Now,
                        IslemGerceklestiMi = true,
                        Tutar = entity.OgrenciSozlesmeOdemeBilgi.PesinatTutar,
                        Aciklama = Core.Properties.ServiceNoticesResources.PesinatAciklama,
                        PersonelId = Identity.PersonelId
                    };

                    pesinatTutarHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                    {
                        HesapHareket = pesinatTutarHareket,
                        OgrenciSozlesmeId = entity.OgrenciSozlesmeId
                    };

                    pesinatTutarHareket.AlacakliHesapId = entity.OgrenciId;

                    eklenecekHesapHareketler.Add(pesinatTutarHareket.OgrenciSozlesmeHesapHareket);
                }
            }

            if (eskiModel?.OgrenciSozlesmeHesapHareketler != null && eskiModel.OgrenciSozlesmeHesapHareketler.Count > 0)
            {
                if (odemelerSilinsinMi)
                {
                    silinecekHesapHareketler.AddRange(eskiModel.OgrenciSozlesmeHesapHareketler);
                }
                else
                {
                    foreach (var t in eskiModel.OgrenciSozlesmeHesapHareketler)
                    {
                        if (!t.HesapHareket.IslemGerceklestiMi)
                            silinecekHesapHareketler.Add(t);
                    }
                }
            }

            if (entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler != null && entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count > 0 && eskiModel?.OgrenciSozlesmeHesapHareketler != null && eskiModel?.OgrenciSozlesmeHesapHareketler.Count > 0)
            {
                var odenenAdet = !odemelerSilinsinMi
                    ? eskiModel?.OgrenciSozlesmeHesapHareketler.Count(x => x.HesapHareket.IslemGerceklestiMi && x.HesapHareket.VadeTarihi != null)
                    : 0;

                for (var i = 0; i < entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count; i++)
                {
                    var aylikTaksitBilgi = entity.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler[i];

                    if (aylikTaksitBilgi.TaksitTutari == null || aylikTaksitBilgi.TaksitTutari <= 0 || aylikTaksitBilgi.VadeTarihi == null)
                        continue;

                    var taksitHareket = new HesapHareket
                    {
                        BorcluHesapId = sube.Hesap.HesapId,
                        ParaBirimId = (int)sube.ParaBirimId,
                        VadeTarihi = (DateTime)aylikTaksitBilgi.VadeTarihi,
                        IslemGerceklestiMi = false,
                        Tutar = aylikTaksitBilgi.TaksitTutari,
                        Aciklama = $"{((i + odenenAdet) < 9 ? $"0{(i + odenenAdet + 1).ToString()}" : (i + odenenAdet + 1).ToString())}. {Core.Properties.ServiceNoticesResources.NoluTaksit}",
                        PersonelId = Identity.PersonelId
                    };

                    taksitHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                    {
                        HesapHareket = taksitHareket,
                        OgrenciSozlesmeId = entity.OgrenciSozlesmeId
                    };

                    taksitHareket.AlacakliHesapId = entity.OgrenciId;

                    eklenecekHesapHareketler.Add(taksitHareket.OgrenciSozlesmeHesapHareket);
                }
            }

            if (!odemelerSilinsinMi && entity.ToplamUcret != eskiModel?.ToplamUcret)
            {
                var sozlesmeHesapHareket = eskiModel.OgrenciSozlesmeHesapHareketler.FirstOrDefault(x => x.HesapHareket.AlacakliHesapId == sube.SubeId && x.HesapHareket.IslemGerceklestiMi);

                if (sozlesmeHesapHareket != null)
                {
                    silinecekHesapHareketler.Add(sozlesmeHesapHareket);

                    var ogrenciSozlesmeTur = sozlesmeTurData.GetById(entity.OgrenciSozlesmeTurId);

                    var toplamTutarHareket = new HesapHareket
                    {
                        AlacakliHesapId = sube.Hesap.HesapId,
                        ParaBirimId = sozlesmeHesapHareket.HesapHareket.ParaBirimId,
                        HareketTarihi = sozlesmeHesapHareket.HesapHareket.HareketTarihi,
                        IslemGerceklestiMi = true,
                        Tutar = entity.ToplamUcret,
                        Aciklama = $"{Core.Properties.ServiceNoticesResources.SozlesmeToplamTutar} ({ogrenciSozlesmeTur.OgrenciSozlesmeTurAd} - {entity.OlusturulmaTarihiFormatted})",
                        PersonelId = Identity.PersonelId
                    };

                    toplamTutarHareket.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                    {
                        HesapHareket = toplamTutarHareket,
                        OgrenciSozlesmeId = entity.OgrenciSozlesmeId
                    };

                    toplamTutarHareket.BorcluHesapId = entity.OgrenciId;

                    eklenecekHesapHareketler.Add(toplamTutarHareket.OgrenciSozlesmeHesapHareket);
                }
            }

            if (entity.ToplamOdenen > entity.ToplamUcret)
            {

            }

            var returnMessage = data.UpdatedWithNestedLists(entity, silinecekHesapHareketler, eklenecekHesapHareketler);

            if (!string.IsNullOrEmpty(returnMessage))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = returnMessage
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<List<OgrenciSozlesme>> UpdateList(List<OgrenciSozlesme> entities)
        {
            var entityOperationResult = new EntityOperationResult<List<OgrenciSozlesme>>(entities);

            var returnMessage = data.UpdateList(entities);

            if (!string.IsNullOrEmpty(returnMessage))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = returnMessage
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<OgrenciSozlesme> DeleteById(int id)
        {
            var newParameters = new List<Parameter>()
            {
                new Parameter("OgrenciSozlesmeId", id)
            };

            var entityOperationResult = new EntityOperationResult<OgrenciSozlesme>();

            var returnCount = dpData.ExecuteGetRowCount("OgrenciSozlesme_Sil", newParameters);

            if (returnCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<OgrenciSozlesme, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public OgrenciSozlesme Get(Expression<Func<OgrenciSozlesme, bool>> expression = null, params Expression<Func<OgrenciSozlesme, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<OgrenciSozlesme> List(Expression<Func<OgrenciSozlesme, bool>> expression = null, params Expression<Func<OgrenciSozlesme, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSozlesme>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public EntityPagedDataSource<OgrenciSozlesme> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<OgrenciSozlesmeDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            var entityPagedDataSource = dpData.GetPagedEntities("OgrenciSozlesme_Listele", filter, newParameters);

            return entityPagedDataSource;
        }
    }
}
