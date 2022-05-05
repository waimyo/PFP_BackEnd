using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Enumerations
{
    public enum AuditAction
    {
        QUERY=1,
        ADD,
        UPDATE,
        DELETE,
        PRINT,
        LOGIN,
        LOGOFF,
        CARDAPPROVE,
        CARDREQUEST,
        CARDUSAGE
    }
}
