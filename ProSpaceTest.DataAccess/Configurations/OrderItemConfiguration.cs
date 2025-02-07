using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.OrderId).IsRequired();
            builder.Property(oi => oi.ItemId).IsRequired();
            builder.Property(oi => oi.ItemsCount).IsRequired();

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId);
        }
    }
}
