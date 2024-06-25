using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object? Result {  get; set; }
    }
}
