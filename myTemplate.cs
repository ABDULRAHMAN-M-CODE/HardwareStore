using System;

public class MyTemplate
{
	public MyTemplate()
	{
        List<JM>[] newAndObsolete = GetNewAndObsoleteJunctionEntities<JM>
            (List < JM > objects, List < JM > existingEntities);
        if (newAndObsolete[0].Count != 0)
        {
            _context.AddRange(newAndObsolete[0]);
        }
        if (newAndObsolete[1].Count != 0)
        {
            _context.RemoveRange(newAndObsolete[1]);
        }
        CreateLookupTable("Product","Manufacturer")
    }
}
