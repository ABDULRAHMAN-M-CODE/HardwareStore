using HardwareStore.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace HardwareStore.Services
{
    /// <summary>
    /// A run-time generated database entries that cannot be registerd into the DI container.
    /// </summary>
    public class AuditableEntries: IAuditableEntries<EntityEntry>
    {
        private readonly IEnumerable<EntityEntry> _entitiesEntries;
        public AuditableEntries(IEnumerable<EntityEntry> auditablEntries)
        {
             _entitiesEntries=auditablEntries;
        }
        #region public methods
        #endregion
        public void AuditTimeAndActor(string actorId)
        {


            foreach (EntityEntry entry in _entitiesEntries)
            {
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    var x = (AuditableEntity)entry.Entity;
                    ((AuditableEntity)entry.Entity).CreatedAt = now;
                    ((AuditableEntity)entry.Entity).UpdatedAt = now;
                    ((AuditableEntity)entry.Entity).CreatorId = actorId;
                    ((AuditableEntity)entry.Entity).UpdaterId = actorId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    ((AuditableEntity)entry.Entity).UpdatedAt = now;
                    ((AuditableEntity)entry.Entity).UpdaterId = actorId;


                }
                else if (entry.State == EntityState.Deleted)
                {
                    ((AuditableEntity)entry.Entity).DeletedAt = now;
                    ((AuditableEntity)entry.Entity).DeleterId = actorId;
                }
                else
                {
                    continue;
                }
            }
        }

    }

}
