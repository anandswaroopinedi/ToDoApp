using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ErrorLogDto
    {
        public string Errormessage { get; set; }

        public string Filename { get; set; }

        public string Methodname { get; set; }

        public string Timestamp { get; set; } 

        public int Linenumber { get; set; }

        public string Stacktrace { get; set; }
    }
}
