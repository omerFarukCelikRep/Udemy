using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Mapping;

public class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.CreatedDate).IsDescending();

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();
        builder.Property(x => x.Status)
               .IsRequired();
        builder.Property(x => x.CreatedBy)
               .HasMaxLength(128)
               .IsRequired();
        builder.Property(x => x.CreatedDate)
               .IsRequired();
        builder.Property(x => x.ModifiedBy)
               .HasMaxLength(128)
               .IsRequired(false);
        builder.Property(x => x.ModifiedDate)
               .IsRequired(false);
    }
}