using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure
{
    public class QueryOptions<TEntity> where TEntity : BaseEntity
    {
        public int fromPage { get; set; }
        public int fromRecord { get; set; }
        public int recordPerPage { get; set; }
        public string SortColumnName { get; set; }
        public QueryOptions()
        {
            SortOrder = SortOrder.ASC;

        }

        public System.Linq.Expressions.Expression<Func<TEntity, bool>> FilterBy { get; set; }
        public List<Func<TEntity, object>> SortBy { get; set; }
        public List<Func<TEntity, object>> SortBy2 { get; set; }
        public SortOrder SortOrder { get; set; }

    }
}
