using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class ResponseStatusEntityTypeConfiguration : IEntityTypeConfiguration<ResponseStatus>
    {
        public void Configure(EntityTypeBuilder<ResponseStatus> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
