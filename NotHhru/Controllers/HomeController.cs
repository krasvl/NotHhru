using NotHhru.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotHhru.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;
        public HomeController(ILogger<HomeController> logger, ApplicationContext db)
        {
            _logger = logger;
            this.db = db;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index(int currentPage = 1)
        {
            ViewBag.currentPage = currentPage;
            if (db.Ads.Count() == 0)
            {
                CreateData();
            }
            IQueryable<Ad> ads = db.Ads;
            return View(ads);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [NonAction]
        private void CreateData()
        {
            List<WorkType> workTypes = new List<WorkType>() {
                new WorkType() { Name = "Полная занятость"},
                new WorkType() { Name = "Подработка"},
                new WorkType() { Name = "Стажировка"},
                new WorkType() { Name = "Частичная занятость"}};
            db.WorkTypes.AddRange(workTypes);

            List<Company> companies = new List<Company>();
            for (int i = 0; i < 50; i++)
            {
                companies.Add(new Company() { Name = $"Кампания {i + 1}" });
            }
            db.Companies.AddRange(companies);

            List<Region> regions = new List<Region>();
            for (int i = 0; i < 20; i++)
            {
                regions.Add(new Region() { Name = $"Регион {i + 1}" });
            }
            db.Regions.AddRange(regions);

            List<Ad> ads = new List<Ad>();
            for (int i = 0; i < 1000; i++)
            {

                ads.Add(new Ad()
                {
                    Name = $"Объявление {i + 1}",
                    Company = companies[new Random().Next(companies.Count() - 1)],
                    Region = regions[new Random().Next(regions.Count() - 1)],
                    Date = new DateTime(2020, new Random().Next(1, 12), new Random().Next(1, 28), new Random().Next(1, 24), 0, 0),
                    Salary = new Random().Next(10000, 500000),
                    WorkType = workTypes[new Random().Next(workTypes.Count() - 1)]
                });
            }
            db.Ads.AddRange(ads);

            db.SaveChanges();
        }
    }
}
