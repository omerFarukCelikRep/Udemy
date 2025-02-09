using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Common.Models.Entities.Base;
using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Persistence.Mapping;
public class SoftDeletableEntityConfiguration<TEntity, TId> : BaseEntityConfiguration<TEntity, TId>
    where TEntity : SoftDeletableEntity<TId>
    where TId : struct
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DeletedBy)
              .HasMaxLength(128)
              .IsRequired(false);
        builder.Property(x => x.DeletedDate)
               .IsRequired(false);

        builder.HasQueryFilter(x => x.Status != Statuses.Deleted);
    }
}
