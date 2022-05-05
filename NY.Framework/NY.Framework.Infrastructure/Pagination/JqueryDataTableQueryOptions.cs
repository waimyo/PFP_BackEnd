using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure
{
    public class JqueryDataTableQueryOptions<TEntity> where TEntity : BaseEntity
    {
        public string Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public JqueryDataTableQueryOptions()
        {
            SortOrder = SortOrder.ASC;

        }

        public string SearchValue { get; set; }
        public System.Linq.Expressions.Expression<Func<TEntity, bool>> FilterBy { get; set; }
        public System.Linq.Expressions.Expression<Func<TEntity, bool>> FilterBy1 { get; set; }
        public List<string> SortColumnsName { get; set; }
        public List<Func<TEntity, object>> SortBy { get; set; }
        public SortOrder SortOrder { get; set; }

    }
}
