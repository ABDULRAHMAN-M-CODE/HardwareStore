using HardwareStore.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace HardwareStore.Services
{
    public class AuditEntriesChanges:IAudit<EntityEntry>
    {
 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditEntriesChanges(IHttpContextAccessor httpContextAccessor)
        {
            
            _httpContextAccessor=httpContextAccessor;
            
        }
        #region public methods
        #endregion
        public void AuditAllChangesAspects(IEnumerable<EntityEntry> auditablEntries)
        {
            

            AuditChangesTime(auditablEntries);
            AuditChangesActor(auditablEntries);
        }
        public void AuditChangesTime(IEnumerable<EntityEntry> auditablEntries)
        {


            foreach (EntityEntry entry in auditablEntries)
            {
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {

                    ((AuditableEntity)entry.Entity).CreatedAt = now;
                    ((AuditableEntity)entry.Entity).UpdatedAt = now;

                }
                else if (entry.State == EntityState.Modified)
                {
                    ((AuditableEntity)entry.Entity).UpdatedAt = now;

                   

                }
                else if (entry.State == EntityState.Deleted)
                {
                    ((AuditableEntity)entry.Entity).DeletedAt = now;

                }
                else
                {
                    continue;
                }
            }
        }
        public void AuditChangesActor(IEnumerable<EntityEntry> auditablEntries)
        {
            foreach (EntityEntry entry in auditablEntries)
            {
                if (entry.State == EntityState.Added)
                {
       
                    ((AuditableEntity)entry.Entity).CreatorId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    ((AuditableEntity)entry.Entity).UpdaterId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                }
                else if (entry.State == EntityState.Modified)
                {
                    ((AuditableEntity)entry.Entity).UpdaterId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                else if(entry.State==EntityState.Deleted)
                {
                    ((AuditableEntity)entry.Entity).DeleterId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                }
                else
                {
                    continue;
                }

            }
            
        }
    }
}
