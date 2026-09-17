namespace HardwareStore.SeedWork
{
    // abstract class is when I want code sharing, not value sharing, all classes will have EnglishName, but they will not have the same value for it
    // and I also was forced to use interface for some syntax reason.
    public interface  IHasEnglishAndArabicName // classes that  extends this will all have the EnglishName property
    {
        public string? EnglishName { get; set; }
        public string? ArabicName { get; set; }
    }
}
