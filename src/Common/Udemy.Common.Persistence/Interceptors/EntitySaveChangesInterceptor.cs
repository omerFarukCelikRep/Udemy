using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Udemy.Common.Models.Entities.Base;
using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Persistence.Interceptors;
public class EntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private const string Authorization = "Authorization";
    private const string NonAuthorizatedUser = "NonAuthorizatedUser";
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData?.Context is not null)
        {
            AssignBaseProperties(eventData.Context);
        }

        return base.SavedChangesAsync(eventData!, result, cancellationToken);
    }

    private void AssignBaseProperties(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<BaseEntity>();
        var accessor = context.GetService<IHttpContextAccessor>();
        var token = accessor?.HttpContext?.Request?.Headers[Authorization]
                                                   .FirstOrDefault()?.Split(" ")
                                                   .LastOrDefault();
        string userId = token is not null ? AuthorizationHelper.GetUserId(token) : NonAuthorizatedUser;

        foreach (var entry in entries)
        {
            SetIfAdded(entry,userId);
        }
    }

    private static void SetIfAdded(EntityEntry<BaseEntity> entry, string userId)
    {
        if (entry?.State is not EntityState.Added)
            return;

        entry.Entity.CreatedDate = DateTime.Now;
        entry.Entity.CreatedBy = userId;
        entry.Entity.Status = Statuses.Added;
    }

    private static void SetIfModified(EntityEntry<BaseEntity> entry, string userId)
    {
        if (entry?.State is not EntityState.Modified)
            return;

        entry.Entity.ModifiedDate = DateTime.Now;
        entry.Entity.ModifiedBy = userId;
        entry.Entity.Status |= Statuses.Modified;
    }
}
