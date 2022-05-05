using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Commands
{
    public class CommandResult<TEntity> :ICommandResult where TEntity : BaseEntity 
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }

        public List<string> Messages { get; set; }
        public List<TEntity> Result { get; set; }
        public int Id { get; set; }
    

        public CommandResult()
        {
            Success = false;
            Messages = new List<string>();
            Result = new List<TEntity>();
        }

    }
    
}
