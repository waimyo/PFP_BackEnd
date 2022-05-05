using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.EntityTypeConfigurations
{
    public class ISP_GroupMemberListEntityTypeConfiguration : IEntityTypeConfiguration<SP_GroupMemberList>
    {
        public void Configure(EntityTypeBuilder<SP_GroupMemberList> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
