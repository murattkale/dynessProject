using Core.Entities.Dto;
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
    public class KurumOgrenciSozlesmeMetinManager : IKurumOgrenciSozlesmeMetinService
    {
        IEfKurumOgrenciSozlesmeMetinData data;

        public KurumOgrenciSozlesmeMetinManager(IEfKurumOgrenciSozlesmeMetinData data)
        {
            this.data = data;
        }

        public EntityOperationResult<KurumOgrenciSozlesmeMetin> Add(KurumOgrenciSozlesmeMetin entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<KurumOgrenciSozlesmeMetin> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<KurumOgrenciSozlesmeMetin> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public KurumOgrenciSozlesmeMetin Get(Expression<Func<KurumOgrenciSozlesmeMetin, bool>> expression = null, params Expression<Func<KurumOgrenciSozlesmeMetin, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<KurumOgrenciSozlesmeMetin, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KurumOgrenciSozlesmeMetin> List(Expression<Func<KurumOgrenciSozlesmeMetin, bool>> expression = null, params Expression<Func<KurumOgrenciSozlesmeMetin, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<KurumOgrenciSozlesmeMetin> Update(KurumOgrenciSozlesmeMetin entity)
        {
            throw new NotImplementedException();
        }
    }
}
