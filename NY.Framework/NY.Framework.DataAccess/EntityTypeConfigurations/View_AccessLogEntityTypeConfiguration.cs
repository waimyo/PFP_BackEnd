using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class View_AccessLogEntityTypeConfiguration : IEntityTypeConfiguration<View_AccessLog>
    {
        public void Configure(EntityTypeBuilder<View_AccessLog> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
