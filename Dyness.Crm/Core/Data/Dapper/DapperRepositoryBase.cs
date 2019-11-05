using Core.Entities.Dto;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.Dapper
{
    public class DapperRepositoryBase<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        private string connectionString;

        public DapperRepositoryBase()
        {
            connectionString = ConfigurationManager.ConnectionStrings["EFContext"].ConnectionString;
        }

        public void Execute(string command, IEnumerable<Parameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = null;

                if (parameters != null && parameters.Count() > 0)
                {
                    dynamicParameters = new DynamicParameters();

                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Name, parameter.Value, direction: parameter.Direction);
                    }
                }

                connection.Execute(command, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int ExecuteGetRowCount(string command, IEnumerable<Parameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Name, parameter.Value, direction: parameter.Direction);
                    }
                }

                var affectedRowsCount = (int)connection.ExecuteScalar(command, dynamicParameters, commandType: CommandType.StoredProcedure);

                return affectedRowsCount;
            }
        }

        public TEntity GetEntity(string command, IEnumerable<Parameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add($"@{parameter.Name}", parameter.Value, direction: parameter.Direction);
                    }
                }

                return connection.QueryFirstOrDefault<TEntity>(command, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<TEntity> GetEntities(string command, IEnumerable<Parameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add($"@{parameter.Name}", parameter.Value, direction: parameter.Direction);
                    }
                }

                return connection.Query<TEntity>(command, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public EntityPagedDataSource<TEntity> GetPagedEntities(string command, EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                var newParams = EntityPagedDataSourceFilter.ToParameters(filter).ToList();

                if (parameters != null)
                    newParams.AddRange(parameters);

                foreach (var parameter in newParams)
                {
                    dynamicParameters.Add($"@{parameter.Name}", parameter.Value, direction: parameter.Direction);
                }

                EntityPagedDataSource<TEntity> entityPagedDataSource = new EntityPagedDataSource<TEntity>();

                using (var multiQuery = connection.QueryMultiple(command, dynamicParameters, commandType: CommandType.StoredProcedure))
                {
                    entityPagedDataSource.data = multiQuery.Read<TEntity>().ToList();
                    entityPagedDataSource.draw = multiQuery.Read<int>().First();
                    entityPagedDataSource.recordsTotal = multiQuery.Read<int>().First();
                    entityPagedDataSource.recordsFiltered = multiQuery.Read<int>().First();
                }

                return entityPagedDataSource;
            }
        }
    }
}