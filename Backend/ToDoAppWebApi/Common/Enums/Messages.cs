using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    
    public enum Messages
    {
        [Description("Successfully completed")] Success = 1,
        [Description("Something Went wrong")] Exception = 2,
        [Description("Operation Failed")] Failure = 3,
    }
}
