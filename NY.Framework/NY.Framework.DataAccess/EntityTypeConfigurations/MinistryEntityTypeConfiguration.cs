using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class MinistryEntityTypeConfiguration : IEntityTypeConfiguration<Ministry>
    {
        public void Configure(EntityTypeBuilder<Ministry> builder)
        {
        }
    }
}
