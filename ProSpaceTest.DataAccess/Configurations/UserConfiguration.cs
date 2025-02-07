using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            // 1. Указываем имя таблицы
            builder.ToTable("User");

            // 2. Настройка первичного ключа
            builder.HasKey(u => u.Id);

            // 3. Настройка свойств
            builder.Property(u => u.Login)
                   .IsRequired() // Обязательное поле
                   .HasMaxLength(100); // Максимальная длина строки

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Firstname)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Lastname)
                   .HasMaxLength(100); // Необязательное поле (nullable)

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true); // Значение по умолчанию: true

            builder.Property(u => u.Created)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Значение по умолчанию: текущая дата и время

            builder.Property(u => u.LastLogin)
                   .IsRequired(false); // Nullable поле

            // 4. Настройка внешнего ключа и связи с Customer
            builder.HasOne(u => u.Customer) // Один пользователь связан с одним заказчиком
                   .WithMany()              // У заказчика может быть много пользователей (если нужно)
                   .HasForeignKey(u => u.CustomerId) // Внешний ключ
                   .OnDelete(DeleteBehavior.Restrict); // Защита от каскадного удаления

            // 5. Создание индекса для CustomerId
            builder.HasIndex(u => u.CustomerId)
                   .IsUnique(); // Уникальный индекс
        }
    }
}