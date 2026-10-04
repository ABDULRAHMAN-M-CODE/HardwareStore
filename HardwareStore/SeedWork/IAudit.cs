using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace HardwareStore.SeedWork
{
    public interface IAuditableEntries<T>
    {
       
        //public void AuditAllChangesAspects(IEnumerable<T> auditableEntries, string manipulatorId); // optional facade
        public void AuditChangesTime();
        
        public void AuditChangesActor(string manipulatorId); 


    }
}
