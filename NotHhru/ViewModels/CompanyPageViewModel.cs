using NotHhru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.ViewModels
{
    public class CompanyPageViewModel
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public IQueryable<Ad> Ads { get; set; }

        public CompanyPageViewModel(string name, string img, IQueryable<Ad> ads)
        {
            Name = name;
            Img = img;
            Ads = ads;
        }
    }
}
