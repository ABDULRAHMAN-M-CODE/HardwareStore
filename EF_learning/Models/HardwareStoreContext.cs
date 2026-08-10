
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
// add a reference to System.ComponentModel.DataAnnotations DLL
using System.ComponentModel.DataAnnotations;

using System.Reflection.Emit;

namespace Intro
{


    public class HardwareStoreContext: DbContext
    {


        public HardwareStoreContext(DbContextOptions <HardwareStoreContext> options):base(options)
        {
            
        }


    }


}
