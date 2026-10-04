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
        private readonly IEnumerable<EntityEntry> _auditablEntries;
        public AuditableEntries(IEnumerable<EntityEntry> auditablEntries)
        {
             _auditablEntries=auditablEntries;
        }
        #region public methods
        #endregion
            
        public void AuditChangesTime()
        {


            foreach (EntityEntry entry in _auditablEntries)
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
        public void AuditChangesActor(string manipulatorId)
        {
            foreach (EntityEntry entry in _auditablEntries)
            {
                if (entry.State == EntityState.Added)
                {
       
                    ((AuditableEntity)entry.Entity).CreatorId = manipulatorId;
                    ((AuditableEntity)entry.Entity).UpdaterId = manipulatorId;

                }
                else if (entry.State == EntityState.Modified)
                {
                    ((AuditableEntity)entry.Entity).UpdaterId =  manipulatorId;
                }
                else if(entry.State==EntityState.Deleted)
                {
                    ((AuditableEntity)entry.Entity).DeleterId =  manipulatorId;

                }
                else
                {
                    continue;
                }

            }
            
        }
    }

}
