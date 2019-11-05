using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Abstract
{
    public interface IOgrenciService : IServiceBase, IServiceModel<Ogrenci>
    {
        Ogrenci GetBilgi(Expression<Func<Ogrenci, bool>> expression = null, params Expression<Func<Ogrenci, object>>[] includes);

        string GetSonOgrenciNo();

        EntityPagedDataSource<OgrenciDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
