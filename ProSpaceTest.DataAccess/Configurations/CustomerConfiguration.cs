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
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.HasKey(с => с.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(9)
            .IsUnicode()
            .HasAnnotation("RegularExpression", @"^\d{4}-\d{4}$");

            builder.Property(c => c.Address)
                .HasMaxLength(500);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            
            builder.HasIndex(c => c.Code)
                .IsUnique(); 
        }
    }
}
