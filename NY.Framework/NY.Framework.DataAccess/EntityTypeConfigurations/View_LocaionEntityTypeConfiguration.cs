using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class View_LocaionEntityTypeConfiguration : IEntityTypeConfiguration<View_Location>
    {
        public void Configure(EntityTypeBuilder<View_Location> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
