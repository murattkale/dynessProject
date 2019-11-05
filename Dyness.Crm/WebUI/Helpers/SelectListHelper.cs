using Core.CrossCuttingConcerns.Security;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebUI.Helpers
{
    public class SelectListHelper
    {
        private IServiceFactory serviceFactory;

        public SelectListHelper(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        #region Private Methods

        private string GetValue<TEntity, TMember>(TEntity entity, Expression<Func<TEntity, TMember>> expression)
        {
            MemberExpression memberExpr = (MemberExpression)expression.Body;
            string memberName = memberExpr.Member.Name;
            Func<TEntity, TMember> compiledDelegate = expression.Compile();
            TMember value = compiledDelegate(entity);

            return value.ToString();
        }

        private List<SelectListItem> GetSelectListItems<TEntity, TMember>(
            IEnumerable<TEntity> entities,
            Expression<Func<TEntity, TMember>> expressionId,
            Expression<Func<TEntity, string>> expressionValue,
            Expression<Func<TEntity, string>> expressionGroupValue = null,
            int[] selectedItems = null)
        {
            var selectListGroupList = new List<SelectListGroup>();

            var selectItemList = new List<SelectListItem>();

            foreach (var entity in entities)
            {
                var id = GetValue(entity, expressionId);
                var text = GetValue(entity, expressionValue);

                var selectItem = new SelectListItem
                {
                    Value = id,
                    Text = text,
                    Selected = selectedItems != null && selectedItems.Count(x => x == Convert.ToInt32(id)) > 0
                };

                if (expressionGroupValue != null)
                {
                    var groupName = GetValue(entity, expressionGroupValue);

                    var group = selectListGroupList.FirstOrDefault(x => string.Equals(x.Name, groupName));

                    if (group != null)
                    {
                        selectItem.Group = group;
                    }
                    else
                    {
                        group = new SelectListGroup { Name = groupName };
                        selectListGroupList.Add(group);
                    }

                    selectItem.Group = group;
                }

                selectItemList.Add(selectItem);
            }

            return selectItemList;
        }

        public List<SelectListItem> ToSelectList<TEntity, TMember>(
            IEnumerable<TEntity> entities,
            Expression<Func<TEntity, TMember>> expressionId,
            Expression<Func<TEntity, string>> expressionValue,
            Expression<Func<TEntity, string>> expressionGroupValue = null,
            int[] selectedItems = null)
        {
            var selectItemList = GetSelectListItems(entities, expressionId, expressionValue, expressionGroupValue, selectedItems);

            return selectItemList;
        }

        #endregion

        #region Selects

        public List<SelectListItem> AySelectList()
        {
            var months = new List<Tuple<string, string>>();

            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var year = DateTime.Now.Year;

            for (int i = 0; i < 12; i++)
            {
                var count = i + 1;

                months.Add(new Tuple<string, string>(
                    (i + 1).ToString(),
                    $"{(count < 10 ? "0" + count.ToString() : count.ToString())} - {new DateTime(year, i + 1, 1).ToString("MMMM", culture)}"));
            }

            return ToSelectList(months, x => x.Item1, x => x.Item2);
        }

        public List<SelectListItem> AyHaftaGunSelectList()
        {
            var tur = new List<Tuple<string, string>>();

            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var year = DateTime.Now.Year;

            tur.Add(new Tuple<string, string>("1", Resources.LangResources.Bugun));
            tur.Add(new Tuple<string, string>("2", Resources.LangResources.BuHafta));
            tur.Add(new Tuple<string, string>("3", Resources.LangResources.BuAy));

            return ToSelectList(tur, x => x.Item1, x => x.Item2);
        }

        public List<SelectListItem> BankaSelectList(bool etkinMi = true)
        {
            Expression<Func<Banka, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IBankaService>();
            var list = service.List(expression).OrderBy(x => x.BankaAd);

            return ToSelectList(list, x => x.BankaId, x => x.BankaAd);
        }

        public List<SelectListItem> BankaHesapSelectList(bool etkinMi = true)
        {
            Expression<Func<BankaHesap, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IBankaHesapService>();
            var list = service.List(expression, y => y.Banka).OrderBy(x => x.Banka.BankaId);

            return ToSelectList(list, x => x.BankaHesapId, x => x.AciklamaFormatted);
        }

        public List<SelectListItem> BransSelectList(bool etkinMi = true)
        {
            Expression<Func<Brans, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IBransService>();

            if (Identity.KurumId == -1)
            {
                var branslar = service.List(expression, y => y.Kurum).OrderBy(x => x.BransAd);

                var tuppleList = new List<Tuple<int, string>>();

                foreach (var brans in branslar)
                {
                    tuppleList.Add(new Tuple<int, string>(brans.BransId, $"{brans.BransAd}{(brans.Kurum != null ? $" - {brans.Kurum.KurumAd}" : "")}"));
                }

                return ToSelectList(tuppleList, x => x.Item1, x => x.Item2);
            }
            else
            {
                var branslar = service.List(expression).OrderBy(x => x.BransAd);

                return ToSelectList(branslar, x => x.BransId, x => x.BransAd);
            }
        }

        public List<SelectListItem> DersSelectList(int? bransId, bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<Ders, bool>> expression = null;

            if (bransId == null)
            {
                if (etkinMi)
                    expression = x => x.EtkinMi && x.BransDersler.Count(y => y.DersId == x.DersId) == 0;
                else
                    expression = x => x.BransDersler.Count(y => y.DersId == x.DersId) == 0;
            }
            else
            {
                if (etkinMi)
                    expression = x => x.EtkinMi && x.BransDersler.Count(y => y.DersId == x.DersId && y.BransId == bransId) > 0;
                else
                    expression = x => x.BransDersler.Count(y => y.DersId == x.DersId && y.BransId == bransId) > 0;
            }

            var service = serviceFactory.CreateService<IDersService>();
            var list = service.List(expression).OrderBy(x => x.DersAd);
            return ToSelectList(list, x => x.DersId, x => x.DersAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> DersSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<Ders, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IDersService>();
            var list = service.List(expression).OrderBy(x => x.DersAd);
            return ToSelectList(list, x => x.DersId, x => x.DersAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> DersGrupSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<DersGrup, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IDersGrupService>();
            var list = service.List(expression).OrderBy(x => x.DersGrupAd);
            return ToSelectList(list, x => x.DersGrupId, x => x.DersGrupAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> DersKonuSelectList(int dersId, int[] selectedItems = null)
        {
            Expression<Func<Konu, bool>> expression = x => x.EtkinMi && x.DersId == dersId;
            var service = serviceFactory.CreateService<IKonuService>();
            var list = service.List(expression).OrderBy(x => x.Baslik);
            return ToSelectList(list, x => x.KonuId, x => x.Baslik, selectedItems: selectedItems);
        }

        public List<SelectListItem> DerslikSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<Derslik, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IDerslikService>();
            var list = service.List(expression).OrderBy(x => x.DerslikAd);

            return ToSelectList(list, x => x.DerslikId, x => x.DerslikAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> DersVideoKategoriSelectList(int dersId, int[] selectedItems = null)
        {
            Expression<Func<VideoKategori, bool>> expression = x => x.EtkinMi && x.DersId == dersId;
            var service = serviceFactory.CreateService<IVideoKategoriService>();
            var list = service.List(expression).OrderBy(x => x.VideoKategoriAd);
            return ToSelectList(list, x => x.VideoKategoriId, x => x.VideoKategoriAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> EhliyetTurSelectList(bool etkinMi = true)
        {
            Expression<Func<EhliyetTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IEhliyetTurService>();
            var list = service.List(expression).OrderBy(x => x.EhliyetTurAd);

            return ToSelectList(list, x => x.EhliyetTurId, x => x.EhliyetTurAd);
        }

        public List<SelectListItem> EtkinlikSelectList(bool sonBasvuruTarihiUygunMu = true, bool kontenjanVarMi = true)
        {
            Expression<Func<Etkinlik, bool>> expression = null;

            if (sonBasvuruTarihiUygunMu)
            {
                var etkinlikSonBasvuruTarihi = DateTime.Now.Date;
                expression = x => x.EtkinlikSonBasvuruTarihi >= etkinlikSonBasvuruTarihi;
            }

            Func<Etkinlik, bool> expression2 = null;

            if (kontenjanVarMi)
                expression2 = x => x.KontenjanVarmi;

            var service = serviceFactory.CreateService<IEtkinlikService>();
            var list = service.List(expression).Where(expression2).OrderBy(x => x.EtkinlikAd);

            return ToSelectList(list, x => x.EtkinlikId, x => x.EtkinlikAd);
        }

        public List<SelectListItem> GenelHesapTurSelectList()
        {
            var hesapTurler = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("1", Resources.LangResources.KasaHesabi),
                new Tuple<string, string>("2", Resources.LangResources.BankaHesabi),
                new Tuple<string, string>("3", Resources.LangResources.CekHesabi),
                new Tuple<string, string>("4", Resources.LangResources.GelirHesabi),
                new Tuple<string, string>("5", Resources.LangResources.GiderHesabi)
            };

            return ToSelectList(hesapTurler, x => x.Item1, x => x.Item2);
        }

        public List<SelectListItem> HesapParaBirimSelectList(int paraBirimId, int hesapTurGrupId)
        {
            Expression<Func<Hesap, bool>> expression = x =>
                x.ParaBirimId == paraBirimId &&
                x.UstHesapId == null &&
                x.HesapTur.HesapTurGrupId == hesapTurGrupId &&
                x.HesapTur.HesapTurGrupId != 2;

            var service = serviceFactory.CreateService<IHesapService>();
            var list = service.List(expression,
                    y => y.BagliKurum,
                    y => y.HesapTur).
                OrderBy(x => x.HesapTur.HesapTurAd).
                ThenBy(x => x.HesapBaslik).
                ToList();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].HesapBaslik = $"{list[i].BagliKurumAd} - {list[i].HesapTurAd} - {list[i].HesapBaslik}";
            }

            return ToSelectList(list, x => x.HesapId, x => x.HesapBaslik);
        }

        public List<SelectListItem> HesapParaBirimAltHesapSelectList(int paraBirimId, int ustHesapId, int hesapTurGrupId)
        {
            Expression<Func<Hesap, bool>> expression = null;

            if (hesapTurGrupId > 0)
            {
                expression = x =>
                    x.ParaBirimId == paraBirimId &&
                    x.UstHesapId == ustHesapId &&
                    x.HesapTur.HesapTurGrupId == hesapTurGrupId;
            }
            else
            {
                expression = x =>
                    x.ParaBirimId == paraBirimId &&
                    x.UstHesapId == ustHesapId &&
                    x.HesapTur.HesapTurGrupId != 1;
            }

            var service = serviceFactory.CreateService<IHesapService>();
            var list = service.
                List(
                expression,
                y => y.HesapTur).
                OrderBy(x => x.HesapTurAd).
                ThenBy(x => x.HesapBaslik).
                ToList();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].HesapBaslik = $"{list[i].HesapTurAd} - {list[i].HesapBaslik}";
            }

            return ToSelectList(list, x => x.HesapId, x => x.HesapBaslik);
        }

        public List<SelectListItem> HesapParaBirimHesapTurIdAltHesapSelectList(int paraBirimId, int ustHesapId, int hesapTurId)
        {
            Expression<Func<Hesap, bool>> expression = x => x.ParaBirimId == paraBirimId && x.UstHesapId == ustHesapId && x.HesapTurId == hesapTurId;

            var service = serviceFactory.CreateService<IHesapService>();
            var list = service.
                List(
                    expression,
                    y => y.HesapTur).
                OrderBy(x => x.HesapTurAd).
                ThenBy(x => x.HesapBaslik).
                ToList();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].HesapBaslik = $"{list[i].HesapTurAd} - {list[i].HesapBaslik}";
            }

            return ToSelectList(list, x => x.HesapId, x => x.HesapBaslik);
        }

        public List<SelectListItem> HareketTurSelectList()
        {
            var hareketTurler = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("0",Resources.LangResources.Hepsi),
                new Tuple<string, string>("1",Resources.LangResources.Gelir),
                new Tuple<string, string>("2",Resources.LangResources.Gider)
            };

            return ToSelectList(hareketTurler, x => x.Item1, x => x.Item2);
        }

        public List<SelectListItem> HesapTurSelectList(int hesapTurGrupId, bool hepsiniListele = false, bool etkinMi = true)
        {
            Expression<Func<HesapTur, bool>> expression = null;

            if (hesapTurGrupId != 0)
            {
                if (etkinMi)
                    expression = x => x.EtkinMi && x.HesapTurGrupId == hesapTurGrupId;
                else
                    expression = x => x.HesapTurGrupId == hesapTurGrupId;
            }
            else if (etkinMi)
                expression = x => x.EtkinMi && x.HesapTurGrupId == hesapTurGrupId;

            var service = serviceFactory.CreateService<IHesapTurService>();
            var list = service.
                List(expression).
                OrderBy(x => x.HesapTurAd).
                ToList();

            if (!hepsiniListele)
            {
                list = list.
                    Where(x => !new List<int> { 1, 2, 3 }.Contains(x.HesapTurId)).
                    OrderBy(x => x.HesapTurAd).
                    ToList();
            }

            return ToSelectList(list, x => x.HesapTurId, x => x.HesapTurAd);
        }

        public List<SelectListItem> HesapTurGrupSelectList(bool etkinMi = true)
        {
            Expression<Func<HesapTurGrup, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IHesapTurGrupService>();
            var list = service.List(expression).OrderBy(x => x.HesapTurGrupId);

            return ToSelectList(list, x => x.HesapTurGrupId, x => x.HesapTurGrupAd);
        }

        public List<SelectListItem> IlceSelectList(int sehirId, bool etkinMi = true)
        {
            Expression<Func<Ilce, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi && x.SehirId == sehirId;
            else
                expression = x => x.SehirId == sehirId;

            var service = serviceFactory.CreateService<IIlceService>();
            var list = service.List(expression).OrderBy(x => x.IlceAd);

            return ToSelectList(list, x => x.IlceId, x => x.IlceAd);
        }

        public List<SelectListItem> KiyafetBedenSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<KiyafetBeden, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IKiyafetBedenService>();
            var list = service.List(expression).OrderBy(x => x.KiyafetBedenAd);

            return ToSelectList(list, x => x.KiyafetBedenId, x => x.KiyafetBedenAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> KiyafetTurSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<KiyafetTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IKiyafetTurService>();
            var list = service.List(expression).OrderBy(x => x.KiyafetTurAd);

            return ToSelectList(list, x => x.KiyafetTurId, x => x.KiyafetTurAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> KonuSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<Konu, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IKonuService>();
            var list = service.List(expression).OrderBy(x => x.Baslik);
            return ToSelectList(list, x => x.KonuId, x => x.Baslik, selectedItems: selectedItems);
        }

        public List<SelectListItem> KurumSelectList(int[] selectedItems = null)
        {
            var service = serviceFactory.CreateService<IKurumService>();
            var list = service.List().OrderBy(x => x.KurumAd);

            return ToSelectList(list, x => x.KurumId, x => x.KurumAd);
        }

        public List<SelectListItem> KurumlarSubeSelectList(int[] kurumIdler = null)
        {
            Expression<Func<Sube, bool>> expression = x => x.EtkinMi && kurumIdler.Contains(x.KurumId);

            var service = serviceFactory.CreateService<ISubeService>();
            var list = service.List(expression, y => y.Kurum).OrderBy(x => x.KurumAd).OrderBy(x => x.SubeAd);

            return ToSelectList(list, x => x.SubeId, x => x.SubeAd, x => x.Kurum.KurumAd);
        }

        public List<SelectListItem> KurumlarSezonSelectList(int[] kurumIdler = null)
        {
            Expression<Func<Sezon, bool>> expression = x => x.EtkinMi && (x.KurumId == null || kurumIdler.Contains((int)x.KurumId));

            var service = serviceFactory.CreateService<ISezonService>();
            var list = service.List(expression, y => y.Kurum).OrderBy(x => x.KurumAd).OrderBy(x => x.SezonAd);

            return ToSelectList(list, x => x.SezonId, x => x.SezonAd, x => x.KurumAd);
        }

        public List<SelectListItem> KurumlarBransSelectList(int[] kurumIdler = null)
        {
            Expression<Func<Sinif, bool>> expression = x => x.Brans.EtkinMi && (x.Brans.KurumId == null || kurumIdler.Contains((int)x.Brans.KurumId));

            var service = serviceFactory.CreateService<ISinifService>();
            var list = service.List(expression, y => y.Brans.Kurum).Select(z => z.Brans).Distinct().OrderBy(x => x.KurumAd).OrderBy(x => x.BransAd);

            return ToSelectList(list, x => x.BransId, x => x.KurumBransAd, x => x.KurumAd);
        }

        public List<SelectListItem> NeredenDuydunuzSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<NeredenDuydunuz, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<INeredenDuydunuzService>();
            var list = service.List(expression).OrderBy(x => x.NeredenDuydunuzBaslik);

            return ToSelectList(list, x => x.NeredenDuydunuzId, x => x.NeredenDuydunuzBaslik, selectedItems: selectedItems);
        }

        public List<SelectListItem> OgrenciSozlesmeTurSelectList(bool etkinMi = true)
        {
            Expression<Func<OgrenciSozlesmeTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IOgrenciSozlesmeTurService>();
            var list = service.List(expression).OrderBy(x => x.OgrenciSozlesmeTurAd);

            return ToSelectList(list, x => x.OgrenciSozlesmeTurId, x => x.OgrenciSozlesmeTurAd);
        }

        public List<SelectListItem> OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList(bool etkinMi = true)
        {
            Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IOgrenciSozlesmeOdemeBilgiSenetImzalayanService>();
            var list = service.List(expression).
                OrderBy(x => x.OgrenciSozlesmeOdemeBilgiSenetImzalayanBilgi);

            return ToSelectList(
                list,
                x => x.OgrenciSozlesmeOdemeBilgiSenetImzalayanId,
                x => x.OgrenciSozlesmeOdemeBilgiSenetImzalayanBilgi);
        }

        public List<SelectListItem> OkulTurSelectList(bool etkinMi = true)
        {
            Expression<Func<OkulTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IOkulTurService>();
            var list = service.List(expression).OrderBy(x => x.OkulTurAd);

            return ToSelectList(list, x => x.OkulTurId, x => x.OkulTurAd);
        }

        public List<SelectListItem> OptikFormSelectList(int[] selectedItems = null)
        {
            Expression<Func<OptikForm, bool>> expression = null;

            var service = serviceFactory.CreateService<IOptikFormService>();
            var list = service.List(expression).OrderBy(x => x.OptikFormAd);

            return ToSelectList(list, x => x.OptikFormId, x => x.OptikFormAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> ParaBirimSelectList(bool etkinMi = true)
        {
            Expression<Func<ParaBirim, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IParaBirimService>();
            var list = service.List(expression).OrderBy(x => x.ParaBirimAd);

            return ToSelectList(list, x => x.ParaBirimId, x => x.ParaBirimAd);
        }

        public List<SelectListItem> PersonelPuantajGunlukDurumSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<PersonelPuantajGunlukDurum, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IPersonelPuantajGunlukDurumService>();
            var list = service.List(expression).OrderBy(x => x.Sira);

            return ToSelectList(
                list,
                x => x.PersonelPuantajGunlukDurumId,
                x => x.PersonelPuantajGunlukDurumAd,
                selectedItems: selectedItems);
        }

        public List<SelectListItem> PersonelSelectList(bool etkinMi = true)
        {
            Expression<Func<Personel, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IPersonelService>();
            var list = service.List(expression).OrderBy(x => x.GrupAdSoyad).OrderBy(x => x.AdSoyad);

            return ToSelectList(list, x => x.PersonelId, x => x.AdSoyad, x => x.PersonelGrup.PersonelGrupAd);
        }

        public List<SelectListItem> PersonelGrupSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<PersonelGrup, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IPersonelGrupService>();
            var list = service.List(expression).OrderBy(x => x.PersonelGrupAd);

            return ToSelectList(list, x => x.PersonelGrupId, x => x.PersonelGrupAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> PersonelYetkiGrupSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<PersonelYetkiGrup, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IPersonelYetkiGrupService>();
            var list = service.List(expression).OrderBy(x => x.PersonelYetkiGrupAd);

            return ToSelectList(list, x => x.PersonelYetkiGrupId, x => x.PersonelYetkiGrupAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> PuanTurSelectList(bool etkinMi = true)
        {
            Expression<Func<PuanTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IPuanTurService>();
            var list = service.List(expression).OrderBy(x => x.PuanTurAd);

            return ToSelectList(list, x => x.PuanTurId, x => x.PuanTurAd);
        }

        public List<SelectListItem> SehirSelectList(int ulkeId, bool etkinMi = true)
        {
            Expression<Func<Sehir, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi && x.UlkeId == ulkeId;
            else
                expression = x => x.UlkeId == ulkeId;

            var service = serviceFactory.CreateService<ISehirService>();
            var list = service.List(expression).OrderBy(x => x.SehirAd);
            return ToSelectList(list, x => x.SehirId, x => x.SehirAd);
        }

        public List<SelectListItem> ServisSelectList(int subeId, bool etkinMi = true, bool kapasiteVarMi = true)
        {
            Expression<Func<Servis, bool>> expression = null;

            if (etkinMi)
                expression = x => x.SubeId == subeId && x.EtkinMi;
            else
                expression = x => x.SubeId == subeId;

            Func<Servis, bool> expression2 = x => x.KapasiteVarmi;

            if (kapasiteVarMi)
                expression2 = x => x.KapasiteVarmi;

            var service = serviceFactory.CreateService<IServisService>();
            var list = service.List(expression).Where(expression2).OrderBy(x => x.ServisAd);

            return ToSelectList(list, x => x.ServisId, x => x.ServisAd);
        }

        public List<SelectListItem> SezonSelectList(bool etkinMi = true)
        {
            Expression<Func<Sezon, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISezonService>();

            if (Identity.KurumId == -1)
            {
                var list = service.List(expression, y => y.Kurum).OrderBy(x => x.SezonAd);

                var tuppleList = new List<Tuple<int, string>>();

                foreach (var sezon in list)
                {
                    tuppleList.Add(new Tuple<int, string>(sezon.SezonId, $"{sezon.SezonAd}{(sezon.Kurum != null ? $" - {sezon.Kurum.KurumAd}" : "")}"));
                }

                return ToSelectList(tuppleList, x => x.Item1, x => x.Item2);
            }
            else
            {
                var list = service.List(expression).OrderBy(x => x.SezonAd);

                return ToSelectList(list, x => x.SezonId, x => x.SezonAd);
            }

        }

        public List<SelectListItem> SinavSelectList()
        {
            var service = serviceFactory.CreateService<ISinavService>();
            var list = service.List().OrderByDescending(x => x.SinavTarihi);

            return ToSelectList(list, x => x.SinavId, x => x.Baslik);
        }

        public List<SelectListItem> SinavTurSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<SinavTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISinavTurService>();
            var list = service.List(expression).OrderBy(x => x.SinavTurAd);
            return ToSelectList(list, x => x.SinavTurId, x => x.SinavTurAd, selectedItems: selectedItems);
        }

        public List<SelectListItem> SinifSelectList(bool etkinMi = true)
        {
            Expression<Func<Sinif, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISinifService>();
            var list = service.List(expression).OrderBy(x => x.SinifAd);

            return ToSelectList(list, x => x.SinifId, x => x.SinifAd);
        }

        public List<SelectListItem> SinifSelectList(int subeId, int sezonId, int bransId)
        {
            Expression<Func<Sinif, bool>> expression = x =>
                x.SubeId == subeId &&
                x.SezonId == sezonId &&
                x.BransId == (bransId == 0 ? (int?)null : bransId) &&
                x.EtkinMi;

            var service = serviceFactory.CreateService<ISinifService>();
            var list = service.List(expression).OrderBy(x => x.SinifAd);

            return ToSelectList(list, x => x.SinifId, x => x.SinifAd);
        }

        public List<SelectListItem> SinifSeansSelectList(bool etkinMi = true)
        {
            Expression<Func<SinifSeans, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISinifSeansService>();
            var list = service.List(expression).OrderBy(x => x.SinifSeansAd);

            return ToSelectList(list, x => x.SinifSeansId, x => x.SinifSeansAd);
        }

        public List<SelectListItem> SinifSeviyeSelectList(bool etkinMi = true)
        {
            Expression<Func<SinifSeviye, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISinifSeviyeService>();
            var list = service.List(expression).OrderBy(x => x.SinifSeviyeAd);

            return ToSelectList(list, x => x.SinifSeviyeId, x => x.SinifSeviyeAd);
        }

        public List<SelectListItem> SinifTurSelectList(bool etkinMi = true)
        {
            Expression<Func<SinifTur, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISinifTurService>();
            var list = service.List(expression).OrderBy(x => x.SinifTurAd);

            return ToSelectList(list, x => x.SinifTurId, x => x.SinifTurAd);
        }

        public List<SelectListItem> SmsHesapSelectList(int? smsHesapDurumId = null, bool krediGoster = false)
        {
            Expression<Func<SmsHesap, bool>> expression = null;

            if (smsHesapDurumId != null)
                expression = x => x.SmsHesapDurumId == smsHesapDurumId;

            var service = serviceFactory.CreateService<ISmsHesapService>();
            var list = service.List(expression).OrderBy(x => x.Baslik);

            if (krediGoster)
            {
                var newList = list.Select(x => new Tuple<string, string>(x.SmsHesapId.ToString(), $"{x.Baslik} - ({x.Kredi})")).ToList();
                return ToSelectList(newList, x => x.Item1, x => x.Item2);
            }

            else
                return ToSelectList(list, x => x.SmsHesapId, x => x.Baslik);
        }

        public List<SelectListItem> SmsGonderenGrupSelectList()
        {
            var gruplar = new List<Tuple<string, string>>();
            gruplar.Add(new Tuple<string, string>("1", Resources.LangResources.Ogrenci));
            gruplar.Add(new Tuple<string, string>("2", Resources.LangResources.Personel));

            return ToSelectList(gruplar, x => x.Item1, x => x.Item2);
        }

        public List<SelectListItem> SmsOgrenciGonderenGrupSelectList()
        {
            var gruplar = new List<Tuple<string, string>>();
            gruplar.Add(new Tuple<string, string>("1", Resources.LangResources.Ogrenci));
            gruplar.Add(new Tuple<string, string>("2", Resources.LangResources.VeliEnYakini));
            gruplar.Add(new Tuple<string, string>("3", Resources.LangResources.VeliAnne));
            gruplar.Add(new Tuple<string, string>("4", Resources.LangResources.VeliBaba));
            gruplar.Add(new Tuple<string, string>("5", Resources.LangResources.VeliYakini));

            return ToSelectList(gruplar, x => x.Item1, x => x.Item2, selectedItems: new[] { 1, 2 });
        }

        public List<SelectListItem> SmsHesapDurumSelectList(bool etkinMi = true)
        {
            Expression<Func<SmsHesapDurum, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISmsHesapDurumService>();
            var list = service.List(expression).OrderBy(x => x.SmsHesapDurumAd).OrderBy(x => x.SmsHesapDurumId);

            return ToSelectList(list, x => x.SmsHesapDurumId, x => x.SmsHesapDurumAd);
        }

        public List<SelectListItem> SmsMetinSablonSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<SmsMetinSablon, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISmsMetinSablonService>();
            var list = service.List(expression).OrderBy(x => x.Baslik);

            return ToSelectList(list, x => x.SmsMetinSablonId, x => x.Baslik, selectedItems: selectedItems);
        }

        public List<SelectListItem> SubeAltHesapSelectList(int subeId, int paraBirimId)
        {
            Expression<Func<Hesap, bool>> expression = x =>
                x.UstHesapId == subeId &&
                x.ParaBirimId == paraBirimId &&
                x.HesapTurId != 1 &&
                x.HesapTurId != 2 &&
                x.HesapTurId != 3 &&
                x.HesapTur.HesapTurGrupId == 3;

            var service = serviceFactory.CreateService<IHesapService>();
            var list = service.List(expression).OrderBy(x => x.HesapBaslik);

            return ToSelectList(list, x => x.HesapId, x => x.HesapBaslik);
        }

        public List<SelectListItem> SubeSelectList(bool etkinMi = true, int[] selectedItems = null)
        {
            Expression<Func<Sube, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ISubeService>();
            var list = service.List(expression).OrderBy(x => x.KurumAd).OrderBy(x => x.SubeAd);

            return ToSelectList(list, x => x.SubeId, x => x.SubeAd, x => x.Kurum.KurumAd, selectedItems);
        }

        public List<SelectListItem> SubePersonelSelectList(int subeId)
        {
            Expression<Func<Personel, bool>> expression = x => x.EtkinMi && x.SubeId == subeId;

            var service = serviceFactory.CreateService<IPersonelService>();
            var list = service.List(expression).OrderBy(x => x.GrupAdSoyad).OrderBy(x => x.AdSoyad);

            return ToSelectList(list, x => x.PersonelId, x => x.AdSoyad, x => x.PersonelGrup.PersonelGrupAd);
        }

        public List<SelectListItem> SubeSezonBransSiniflarSelectList(int[] subeIdler, int[] sezonIdler, int[] bransIdler)
        {
            Expression<Func<Sinif, bool>> expression = null;

            if (subeIdler != null && subeIdler.Any() &&
                sezonIdler != null && sezonIdler.Any() &&
                bransIdler != null && bransIdler.Any())
            {
                expression = x => x.EtkinMi &&
                subeIdler.Contains(x.SubeId) &&
                sezonIdler.Contains(x.SezonId) &&
                (x.BransId == null || bransIdler.Contains((int)x.BransId));
            }
            else if (subeIdler != null && subeIdler.Length > 0 &&
                    (sezonIdler == null || sezonIdler.Length == 0) &&
                    (bransIdler == null || bransIdler.Length == 0))
            {
                expression = x => x.EtkinMi && subeIdler.Contains(x.SubeId);
            }
            else if ((subeIdler == null || subeIdler.Length == 0) &&
                    sezonIdler != null && sezonIdler.Length > 0 &&
                    (bransIdler == null || bransIdler.Length == 0))
            {
                expression = x => x.EtkinMi && sezonIdler.Contains(x.SezonId);
            }
            else if ((subeIdler == null || subeIdler.Length == 0) &&
                    (sezonIdler == null || sezonIdler.Length == 0) &&
                    bransIdler != null && bransIdler.Length > 0)
            {
                expression = x => x.EtkinMi && (x.BransId == null || bransIdler.Contains((int)x.BransId));
            }
            else if ((sezonIdler == null || sezonIdler.Length == 0) &&
                sezonIdler != null && sezonIdler.Length > 0 &&
                bransIdler != null && bransIdler.Length > 0)
            {
                expression = x => x.EtkinMi &&
                sezonIdler.Contains(x.SezonId) &&
                (x.BransId == null || bransIdler.Contains((int)x.BransId));
            }
            else if ((sezonIdler == null || sezonIdler.Length == 0) &&
                    (sezonIdler == null || sezonIdler.Length == 0) &&
                    bransIdler != null && bransIdler.Length > 0)
            {
                expression = x => x.EtkinMi &&
                (x.BransId == null || bransIdler.Contains((int)x.BransId));
            }
            else if (subeIdler != null && subeIdler.Length > 0 &&
                    (sezonIdler == null || sezonIdler.Length == 0) &&
                    bransIdler != null && bransIdler.Length > 0)
            {
                expression = x => x.EtkinMi &&
                subeIdler.Contains(x.SubeId) &&
                (x.BransId == null || bransIdler.Contains((int)x.BransId));
            }
            else if (subeIdler != null && subeIdler.Length > 0 &&
                    sezonIdler != null && sezonIdler.Length > 0 &&
                    (bransIdler == null || bransIdler.Length == 0))
            {
                expression = x => x.EtkinMi &&
                subeIdler.Contains(x.SubeId) &&
                sezonIdler.Contains(x.SezonId);
            }
            else
            {
                expression = x => x.EtkinMi;
            }

            var service = serviceFactory.CreateService<ISinifService>();
            var list = service.List(expression, y => y.Brans).Distinct().OrderBy(x => x.BransAd).OrderBy(x => x.SinifAd);

            return ToSelectList(list, x => x.SinifId, x => x.BransSinifAd, x => x.BransAd);
        }

        public List<SelectListItem> TransferTipSelectList(bool etkinMi = true)
        {
            Expression<Func<TransferTip, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<ITransferTipService>();
            var list = service.List(expression).OrderBy(x => x.TransferTipAd);

            return ToSelectList(list, x => x.TransferTipId, x => x.TransferTipAd);
        }

        public List<SelectListItem> UlkeSelectList(bool etkinMi = true)
        {
            Expression<Func<Ulke, bool>> expression = null;

            if (etkinMi)
                expression = x => x.EtkinMi;

            var service = serviceFactory.CreateService<IUlkeService>();
            var list = service.List(expression).OrderBy(x => x.UlkeId);

            return ToSelectList(list, x => x.UlkeId, x => x.UlkeAd);
        }

        public List<SelectListItem> YilSelectList()
        {
            var service = serviceFactory.CreateService<IHesapHareketService>();
            var minimumYear = service.GetMinimumYear();
            var maximumYear = service.GetMaximumYear();

            var years = new List<Tuple<string, string>>();

            for (int i = minimumYear; i < maximumYear + 1; i++)
            {
                years.Add(new Tuple<string, string>(
                   i.ToString(),
                   i.ToString()));
            }

            return ToSelectList(years, x => x.Item1, x => x.Item2);
        }

        #endregion
    }
}