using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations.Home
{
    public class MonthlyPhNoCountEntityTypeConfiguration : IEntityTypeConfiguration<MonthlyPhnoCount>
    {
        public void Configure(EntityTypeBuilder<MonthlyPhnoCount> builder)
        {
           // throw new NotImplementedException();
        }
    }
}
