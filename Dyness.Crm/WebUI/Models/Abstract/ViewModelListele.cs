using Core.Entities.Dto;
using System.Collections.Generic;

namespace WebUI.Models.Abstract
{
    public class ViewModelListele<TEntity> where TEntity : class
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SearchText { get; set; }

        public List<Column> Columns { get; set; }

        public Search Search { get; set; }

        public List<Order> Order { get; set; }

        public TEntity Model { get; set; }

        public EntityPagedDataSource<TEntity> EntityPagedDataSource { get; set; }

        private EntityPagedDataSourceFilter _entityPagedDataSourceFilter;

        public EntityPagedDataSourceFilter EntityPagedDataSourceFilter
        {
            get
            {
                if (_entityPagedDataSourceFilter == null)
                    _entityPagedDataSourceFilter = new EntityPagedDataSourceFilter();

                _entityPagedDataSourceFilter.Draw = Draw;
                _entityPagedDataSourceFilter.Length = Length;
                _entityPagedDataSourceFilter.Start = Start;
                _entityPagedDataSourceFilter.Search = !string.IsNullOrEmpty(SearchText) ? SearchText : (Search != null ? Search.Value : string.Empty);
                _entityPagedDataSourceFilter.SortColumnName = Columns != null && Order != null && !string.IsNullOrEmpty(Columns[Order[0].Column].Name)
                    ? Columns[Order[0].Column].Name
                    : string.Empty;
                _entityPagedDataSourceFilter.SortDirection = Order != null ? Order[0].Dir : string.Empty;

                return _entityPagedDataSourceFilter;
            }
        }
    }

    public class Column
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }

        public string Regex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }

        public string Dir { get; set; }
    }
}