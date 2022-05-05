using NY.Framework.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Commands
{
    public interface IBusinessRule
    {
        bool IsSatisfied();
        string[] GetRules();

    }
}
