using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int Userid { get; set; }

        public int Itemid { get; set; }

        public string ?Createdon { get; set; }

        public string? Completedon { get; set; }

        public int Statusid { get; set; }
        public String?  StatusName { get; set; }
        public int? Isdeleted { get; set; }

    }
}
