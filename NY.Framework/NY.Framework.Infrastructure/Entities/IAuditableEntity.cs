using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    public interface IAuditableEntity
    {
        
        DateTime? CreatedDate { get; set; }
        
        int? CreatedBy { get; set; }
        
        DateTime? ModifiedDate { get; set; }
        
        int? ModifiedBy { get; set; }
    }

}
