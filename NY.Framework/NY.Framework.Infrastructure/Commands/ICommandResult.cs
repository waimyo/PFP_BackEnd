using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Commands
{
    public interface ICommandResult
    {
        bool Success { get; set; }

        List<string> Messages { get; set; }
    }
}
