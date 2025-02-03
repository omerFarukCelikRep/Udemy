using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Udemy.Common.Authentication.Helpers;
using Udemy.Common.Models.Constants;
using Udemy.Common.Models.Entities;
using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Persistence.Interceptors;

public class AuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData?.Context is not null)
        {
            AssignBaseProperties(eventData.Context);
        }

        return base.SavedChangesAsync(eventData!, result, cancellationToken);
    }

    private static void AssignBaseProperties(DbContext context)
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries = context.ChangeTracker.Entries<IAuditableEntity>();
        IHttpContextAccessor accessor = context.GetService<IHttpContextAccessor>();
        string? token = accessor?.HttpContext?.Request?.Headers[Header.Authorization]
                                                   .FirstOrDefault()?.Split(" ")
                                                   .LastOrDefault();
        string userId = token is not null
            ? JwtHelper.GetUser(token)
            : Authorization.NonAuthorizatedUser;

        foreach (EntityEntry<IAuditableEntity> entry in entries)
        {
            SetIfAdded(entry, userId);
            SetIfModified(entry, userId);
        }
    }

    private static void SetIfAdded(EntityEntry<IAuditableEntity> entry, string userId)
    {
        if (entry?.State is not EntityState.Added)
            return;

        entry.Entity.CreatedDate = DateTime.Now;
        entry.Entity.CreatedBy = userId;
        entry.Entity.Status = Statuses.Added;
    }

    private static void SetIfModified(EntityEntry<IAuditableEntity> entry, string userId)
    {
        if (entry?.State is not EntityState.Modified)
            return;

        entry.Entity.ModifiedDate = DateTime.Now;
        entry.Entity.ModifiedBy = userId;
        entry.Entity.Status = Statuses.Modified;
    }
}
