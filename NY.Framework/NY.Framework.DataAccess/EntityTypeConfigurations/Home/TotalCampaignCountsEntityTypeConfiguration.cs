using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations.Home
{
    public class TotalCampaignCountsEntityTypeConfiguration : IEntityTypeConfiguration<TotalCampaignCounts>
    {
        public void Configure(EntityTypeBuilder<TotalCampaignCounts> builder)
        {
           // throw new NotImplementedException();
        }
    }
}
