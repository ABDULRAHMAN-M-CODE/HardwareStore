namespace HardwareStore.SeedWork
{
    //Brand Needs to extend two informations:  TimeStamable and Entity<Brand>
    //But a class may only extend one base class in C#.
    //So, merge both informations in one class → Entity<T> : Timestampable
    public abstract class NonJunctionEntity<T> : Timestampable, IEquatable<T>
        where T : NonJunctionEntity<T>
    {
        public string? EnglishName { get; set; }

        public bool Equals(T other)
        {
            if (other is null)
                return false;

            return EnglishName == other.EnglishName;
        }

        public override bool Equals(object? obj) => Equals(obj as T);
        public override int GetHashCode() => EnglishName.GetHashCode();

    }
}
