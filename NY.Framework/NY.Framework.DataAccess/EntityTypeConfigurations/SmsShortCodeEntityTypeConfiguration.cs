using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class SmsShortCodeEntityTypeConfiguration : IEntityTypeConfiguration<SmsShortCode>
    {
        public void Configure(EntityTypeBuilder<SmsShortCode> builder)
        {
          
        }
    }
}
