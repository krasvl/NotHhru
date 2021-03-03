using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int WorkTypeId { get; set; } 
        public WorkType WorkType { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
