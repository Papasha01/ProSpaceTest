using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.ShipmentDate).IsRequired(false);
            builder.Property(o => o.OrderNumber).IsRequired();
            builder.Property(o => o.Status).IsRequired().HasMaxLength(50);

            builder.HasIndex(i => i.OrderNumber).IsUnique();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
        }
    }
}
