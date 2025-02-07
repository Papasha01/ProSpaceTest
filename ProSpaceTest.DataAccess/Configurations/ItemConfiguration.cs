using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<ItemEntity>
    {
        public void Configure(EntityTypeBuilder<ItemEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Code)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasAnnotation("RegularExpression", @"^\d{2}-\d{4}-[A-Z]{2}\d{2}$");

            builder.Property(i => i.Name).IsRequired().HasMaxLength(250);
            builder.Property(i => i.Category).HasMaxLength(30);

            builder.HasMany(i => i.OrderItems)
                .WithOne(oi => oi.Item)
                .HasForeignKey(oi => oi.ItemId);        
        }
    }
}
