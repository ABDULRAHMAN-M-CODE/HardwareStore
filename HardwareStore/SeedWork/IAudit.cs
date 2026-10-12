using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace HardwareStore.SeedWork
{
    public interface IAuditableEntries<T>
    {
       

        public void AuditTimeAndActor(string actorId);
        



    }
}
