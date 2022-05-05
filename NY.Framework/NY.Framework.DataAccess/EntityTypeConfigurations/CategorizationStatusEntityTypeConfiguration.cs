using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class CategorizationStatusEntityTypeConfiguration : IEntityTypeConfiguration<CategorizationStatus>
    {
        public void Configure(EntityTypeBuilder<CategorizationStatus> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
