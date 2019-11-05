using System.Collections.Generic;

namespace Core.Entities.Dto
{
    public class EntityPagedDataSource<TEntity> where TEntity : class
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public IEnumerable<TEntity> data { get; set; }
    }

    public class EntityPagedDataSourceFilter
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string Search { get; set; }

        public string SortColumnName { get; set; }

        public string SortDirection { get; set; }

        public static IEnumerable<Parameter> ToParameters(EntityPagedDataSourceFilter filter)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Draw", filter.Draw),
                new Parameter("Search", filter.Search),
                new Parameter("Start", filter.Start),
                new Parameter("Length", filter.Length),
                new Parameter("SortColumnName", filter.SortColumnName),
                new Parameter("SortDirection", filter.SortDirection)
            };

            return parameters;
        }
    }
}