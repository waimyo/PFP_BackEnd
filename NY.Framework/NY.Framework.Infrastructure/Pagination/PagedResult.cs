using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Pagination
{
    public class PagedResult<TEntity>
    {
        public List<TEntity> entities { get; set; }
        public int total { get; set; }

        public Pager Pager { get; set; }

        public PagedResult()
        {
            Pager = new Pager();
        }


    }
}
