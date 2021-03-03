using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }


        public List<Ad> Ads { get; set; }
    }
}
