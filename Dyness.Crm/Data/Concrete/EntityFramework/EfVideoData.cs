using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Data.Concrete.EntityFramework
{
    public class EfVideoData : EFEntityRepository<Video, EFContext>, IEfVideoData
    {
        [LogAspect]
        public string AddWithNested(Video entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<Video>().Attach(entity);
                    context.Entry(entity).State = EntityState.Added;

                    if (entity.VideoVideoKategoriler != null)
                    {
                        foreach (var videoVideoKategori in entity.VideoVideoKategoriler)
                        {
                            context.Set<VideoVideoKategori>().Add(videoVideoKategori);
                        }
                    }

                    if (entity.VideoKonular != null)
                    {
                        foreach (var videoKonu in entity.VideoKonular)
                        {
                            context.Set<VideoKonu>().Add(videoKonu);
                        }
                    }

                    if (entity.VideoKurumYetkiler != null)
                    {
                        foreach (var videoKurumYetki in entity.VideoKurumYetkiler)
                        {
                            context.Set<VideoKurumYetki>().Add(videoKurumYetki);
                        }
                    }

                    if (entity.VideoSubeYetkiler != null)
                    {
                        foreach (var videoSubeYetki in entity.VideoSubeYetkiler)
                        {
                            context.Set<VideoSubeYetki>().Add(videoSubeYetki);
                        }
                    }

                    if (entity.VideoSinifYetkiler != null)
                    {
                        foreach (var videoSinifYetki in entity.VideoSinifYetkiler)
                        {
                            context.Set<VideoSinifYetki>().Add(videoSinifYetki);
                        }
                    }

                    context.SaveChanges();

                    returnMessage = string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }

        [LogAspect]
        public string UpdateWithNested(Video entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.VideoVideoKategoriler != null)
                    {
                        foreach (var videoVideoKategori in entity.VideoVideoKategoriler)
                        {
                            if (videoVideoKategori.VideoVideoKategoriId > 0 && videoVideoKategori.Deleted)
                            {
                                context.Set<VideoVideoKategori>().Attach(videoVideoKategori);
                                context.Entry(videoVideoKategori).State = EntityState.Deleted;
                            }
                            else if (videoVideoKategori.VideoVideoKategoriId == 0)
                            {
                                context.Set<VideoVideoKategori>().Add(videoVideoKategori);
                            }
                        }
                    }

                    if (entity.VideoKonular != null)
                    {
                        foreach (var videoKonu in entity.VideoKonular)
                        {
                            if (videoKonu.VideoKonuId > 0 && videoKonu.Deleted)
                            {
                                context.Set<VideoKonu>().Attach(videoKonu);
                                context.Entry(videoKonu).State = EntityState.Deleted;
                            }
                            else if (videoKonu.VideoKonuId == 0)
                            {
                                context.Set<VideoKonu>().Add(videoKonu);
                            }
                        }
                    }

                    if (entity.VideoKurumYetkiler != null)
                    {
                        foreach (var videoKurumYetki in entity.VideoKurumYetkiler)
                        {
                            if (videoKurumYetki.VideoKurumYetkiId > 0 && videoKurumYetki.Deleted)
                            {
                                context.Set<VideoKurumYetki>().Attach(videoKurumYetki);
                                context.Entry(videoKurumYetki).State = EntityState.Deleted;
                            }
                            else if (videoKurumYetki.VideoKurumYetkiId == 0)
                            {
                                context.Set<VideoKurumYetki>().Add(videoKurumYetki);
                            }
                        }
                    }

                    if (entity.VideoSubeYetkiler != null)
                    {
                        foreach (var videoSubeYetki in entity.VideoSubeYetkiler)
                        {
                            if (videoSubeYetki.VideoSubeYetkiId > 0 && videoSubeYetki.Deleted)
                            {
                                context.Set<VideoSubeYetki>().Attach(videoSubeYetki);
                                context.Entry(videoSubeYetki).State = EntityState.Deleted;
                            }
                            else if (videoSubeYetki.VideoSubeYetkiId == 0)
                            {
                                context.Set<VideoSubeYetki>().Add(videoSubeYetki);
                            }
                        }
                    }

                    if (entity.VideoSinifYetkiler != null)
                    {
                        foreach (var videoSinifYetki in entity.VideoSinifYetkiler)
                        {
                            if (videoSinifYetki.VideoSinifYetkiId > 0 && videoSinifYetki.Deleted)
                            {
                                context.Set<VideoSinifYetki>().Attach(videoSinifYetki);
                                context.Entry(videoSinifYetki).State = EntityState.Deleted;
                            }
                            else if (videoSinifYetki.VideoSinifYetkiId == 0)
                            {
                                context.Set<VideoSinifYetki>().Add(videoSinifYetki);
                            }
                        }
                    }

                    context.Set<Video>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

                    context.SaveChanges();

                    returnMessage = string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }
    }
}
