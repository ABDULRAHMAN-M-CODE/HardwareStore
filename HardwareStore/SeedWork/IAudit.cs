using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace HardwareStore.SeedWork
{
    public interface IAudit<T>
    {
        public void AuditAllChangesAspects(IEnumerable<T> auditableEntries); // facade
        public void AuditChangesTime(IEnumerable<T> auditableEntries);
        public void AuditChangesActor(IEnumerable<T> auditableEntries);

        // other behaviors....
    }
}
