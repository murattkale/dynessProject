using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class OgrenciSozlesmeHesapHareketManager : IOgrenciSozlesmeHesapHareketService
    {
        IEfOgrenciSozlesmeHesapHareketData data;

        public OgrenciSozlesmeHesapHareketManager(IEfOgrenciSozlesmeHesapHareketData data)
        {
            this.data = data;

        }

        public EntityOperationResult<OgrenciSozlesmeHesapHareket> Add(OgrenciSozlesmeHesapHareket entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeHesapHareket> Update(OgrenciSozlesmeHesapHareket entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeHesapHareket> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSozlesmeHesapHareket, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public OgrenciSozlesmeHesapHareket Get(Expression<Func<OgrenciSozlesmeHesapHareket, bool>> expression = null, params Expression<Func<OgrenciSozlesmeHesapHareket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OgrenciSozlesmeHesapHareket> List(Expression<Func<OgrenciSozlesmeHesapHareket, bool>> expression = null, params Expression<Func<OgrenciSozlesmeHesapHareket, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<OgrenciSozlesmeHesapHareket> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
