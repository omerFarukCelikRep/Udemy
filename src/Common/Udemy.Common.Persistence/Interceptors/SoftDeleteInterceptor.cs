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
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData?.Context is not null)
            AssignBaseProperties(eventData.Context);

        return base.SavedChangesAsync(eventData!, result, cancellationToken);
    }

    private static void AssignBaseProperties(DbContext context)
    {
        IEnumerable<EntityEntry<ISoftDeletableEntity>> entries = context.ChangeTracker.Entries<ISoftDeletableEntity>();
        IHttpContextAccessor accessor = context.GetService<IHttpContextAccessor>();
        string? token = accessor?.HttpContext?.Request?.Headers[Header.Authorization]
                                                   .FirstOrDefault()?.Split(" ")
                                                   .LastOrDefault();
        string userId = token is not null
            ? JwtHelper.GetUser(token)
            : Authorization.NonAuthorizatedUser;

        foreach (EntityEntry<ISoftDeletableEntity> entry in entries)
        {
            SetIfDeleted(entry, userId);
        }
    }

    private static void SetIfDeleted(EntityEntry<ISoftDeletableEntity> entry, string userId)
    {
        if (entry?.State is not EntityState.Deleted)
            return;

        entry.Entity.DeletedDate = DateTime.Now;
        entry.Entity.DeletedBy = userId;
        entry.Entity.Status = Statuses.Deleted;
    }
}
