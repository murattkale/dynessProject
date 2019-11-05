using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Properties;
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
    public class VideoManager : IVideoService
    {
        IEfVideoData data;
        IDpVideoData dpData;

        public VideoManager(IEfVideoData data, IDpVideoData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<Video> Add(Video entity)
        {
            var entityOperationResult = new EntityOperationResult<Video>(entity);

            var validationResults = EntityValidator<Video>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var returnMessage = data.AddWithNested(entity);

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
                Message = ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Video> Update(Video entity)
        {
            var entityOperationResult = new EntityOperationResult<Video>(entity);

            var validationResults = EntityValidator<Video>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var returnMessage = data.UpdateWithNested(entity);

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
                Message = ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Video> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Video>();

            var entity = data.GetById(id);

            if (Identity.KurumId != -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var returnMessage = data.DeleteById(id);

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
                Message = ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<Video, bool>> expression = null)
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

                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Except(x.VideoSubeYetkiler.Select(y => y.SubeId)).Any()).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => x.VideoKurumYetkiler.Select(y => y.KurumId).Contains(Identity.KurumId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public Video Get(Expression<Func<Video, bool>> expression = null, params Expression<Func<Video, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Except(x.VideoSubeYetkiler.Select(y => y.SubeId)).Any()).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => x.VideoKurumYetkiler.Select(y => y.KurumId).Contains(Identity.KurumId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<Video> List(Expression<Func<Video, bool>> expression = null, params Expression<Func<Video, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Except(x.VideoSubeYetkiler.Select(y => y.SubeId)).Any()).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Video>();

                    predicate = predicate.And(x => x.VideoKurumYetkiler.Select(y => y.KurumId).Contains(Identity.KurumId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public IEnumerable<VideoDto> VideoListele(IEnumerable<Parameter> parameters = null)
        {
            return dpData.GetEntities("Video_Listele", parameters);
        }

        public EntityPagedDataSource<Video> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Video> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Video>
                {
                    data = data.GetAllIncludeMultiple(
                        y => y.Ders)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {

                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<Video>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x =>
                            yetkiliSubeIdler.Except(x.VideoSubeYetkiler.Select(y => y.SubeId)).Any(),
                            y => y.Ders)
                    };
                }
                else
                {
                    entityPagedDataSource = new EntityPagedDataSource<Video>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x =>
                            x.VideoKurumYetkiler.Select(y => y.KurumId).Contains(Identity.KurumId),
                            y => y.Ders)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
