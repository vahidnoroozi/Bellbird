using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bellbird.DAL.Models
{
    public class Alarm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Upvotes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
