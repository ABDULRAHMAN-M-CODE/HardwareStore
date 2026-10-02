namespace HardwareStore.SeedWork
{
    public class RelationshipLookupRecord: IEquatable<RelationshipLookupRecord>
    {
        public int LeftColumnCellValue { get; set; }
        public int RightColumnCellValue { get; set; }
        public bool Equals(RelationshipLookupRecord other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return 

                (other.LeftColumnCellValue== this.LeftColumnCellValue )
                    && 
                (other.RightColumnCellValue==this.RightColumnCellValue);
        }

        public override bool Equals(object? obj) => Equals(obj as RelationshipLookupRecord);
        public override int GetHashCode() => (LeftColumnCellValue,RightColumnCellValue).GetHashCode();
    }
}
