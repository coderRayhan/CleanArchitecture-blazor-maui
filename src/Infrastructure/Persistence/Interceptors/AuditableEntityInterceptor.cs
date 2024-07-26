using Application.Common.Abstractions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;
public class AuditableEntityInterceptor() : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return base.SavingChanges(eventData, result);
    }

    //public void UpdateEntities(DbContext? context)
    //{
    //    if (context == null) return;

    //    //foreach (var entry in context.ChangeTracker.Entries<AuditableEntityBase>)
    //    //{

    //    //}
    //}
}
