using System.Linq.Expressions;

namespace Equinox.Models.DataLayer
{
    public class QueryOptions<T>
    {
        // sorting, filtering, paging
        public Expression<Func<T, object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, bool>> Where { get; set; } = null!;
        public string OrderByDirection { get; set; } = "asc";
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Include strings
        private string[] includes = Array.Empty<string>();
        public string Includes { set => includes = value.Replace(" ", "").Split(','); }
        public string[] GetIncludes() => includes;

        // helpers
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
        public bool HasPaging => PageNumber > 0 && PageSize > 0;
    }
}
