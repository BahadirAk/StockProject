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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Product).WithMany(x => x.Orders).HasForeignKey(x => x.ProductId);
        }
    }
}
