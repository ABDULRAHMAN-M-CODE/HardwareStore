
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
// add a reference to System.ComponentModel.DataAnnotations DLL
using System.ComponentModel.DataAnnotations;

using System.Reflection.Emit;

namespace HardwareStoreNameSpace
{


    public class HardwareStoreDbContext: DbContext
    {


        public HardwareStoreDbContext(DbContextOptions <HardwareStoreDbContext> options):base(options)
        {
            
        }


    }


}
