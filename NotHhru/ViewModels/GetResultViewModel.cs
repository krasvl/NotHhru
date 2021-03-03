using NotHhru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.ViewModels
{
    public class GetResultViewModel
    {
        public IQueryable<Ad> Ads { get; set; }
        public List<WorkType> WorkTypes { get; set; }
        public List<Region> Regions { get; set; }
        public Dictionary<string,string> Categories { get; set; }

        public GetResultViewModel(IQueryable<Ad> ads, List<WorkType> workTypes, List<Region> regions, Dictionary<string, string> categories)
        {
            Ads = ads;
            WorkTypes = workTypes;
            Regions = regions;
            Categories = categories;
        }
    }
}
