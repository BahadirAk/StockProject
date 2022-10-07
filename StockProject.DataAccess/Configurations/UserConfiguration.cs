using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Firstname).HasMaxLength(30).IsRequired();
            builder.Property(x => x.Surname).HasMaxLength(30).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(30).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Balance).HasColumnType("decimal(18,2)").IsRequired();
        }
    }
}
