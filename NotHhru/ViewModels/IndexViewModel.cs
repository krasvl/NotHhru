using NotHhru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.ViewModels
{
    public class IndexViewModel
    {
        public List<Ad> Ads { get; set; }
        public List<Company> Companies { get; set; }

        public IndexViewModel(List<Ad> ads, List<Company> companies)
        {
            Ads = ads;
            Companies = companies;
        }
    }
}
