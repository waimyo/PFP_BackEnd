using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations.Home
{
    public class MonthlyNumberOfPeopleSentAndReceivedSMSEntityTypeConfiguration : IEntityTypeConfiguration<MonthlyNumberOfPeopleSentAndReceivedSMS>
    {
        public void Configure(EntityTypeBuilder<MonthlyNumberOfPeopleSentAndReceivedSMS> builder)
        {
           // throw new NotImplementedException();
        }
    }
}
