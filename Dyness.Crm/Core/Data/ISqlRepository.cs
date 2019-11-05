using Core.Entities.Dto;
using System.Collections.Generic;

namespace Core.Data
{
    public interface ISqlRepository<TEntity> where TEntity : class
    {
        void Execute(string command, IEnumerable<Parameter> parameters = null);

        int ExecuteGetRowCount(string command, IEnumerable<Parameter> parameters = null);

        TEntity GetEntity(string command, IEnumerable<Parameter> parameters = null);

        IEnumerable<TEntity> GetEntities(string command, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<TEntity> GetPagedEntities(string command, EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}