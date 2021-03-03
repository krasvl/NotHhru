﻿using NotHhru.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotHhru.ViewModels;

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

        public IActionResult Index()
        {
            if (db.Ads.Count() == 0)
            {
                CreateData();
            }
            int companiesCount = db.Companies.Count();
            ViewBag.companiesCount = companiesCount;
            int adsCount = db.Ads.Count();
            ViewBag.adsCount = adsCount;
            return View(new IndexViewModel(
                db.Ads.Include(a => a.Company).Include(a => a.Region).Include(a => a.WorkType).OrderByDescending(a => a.Date).ToList(), 
                db.Companies.Include(c => c.Ads).OrderByDescending(c => c.Ads.Count).ToList()));
        }

        private static string _name;
        private static int? _workType;
        private static int? _region;
        private static int? _salary;
        private static Dictionary<string, string> categories = new Dictionary<string, string>();

        public IActionResult GetResult(string name, int? workType, int? region, int? salary, string deleteCategorie, int countRegions = 5, int currentPage = 1)
        {
            if(!string.IsNullOrEmpty(deleteCategorie))
            {
                categories.Remove(deleteCategorie);
                if (deleteCategorie == "Тип работы")
                    _workType = null;
                if (deleteCategorie == "Регион")
                    _region = null;
                if (deleteCategorie == "Зарплата")
                    _salary = null;
            }

            if (name != null ) _name = name;

            if (workType != null)
            {
                _workType = (int)workType;
                _name = "";
                if (categories.ContainsKey("Тип работы")) categories["Тип работы"] = db.WorkTypes.Where(w => w.Id == workType).FirstOrDefault().Name;
                else categories.Add("Тип работы", db.WorkTypes.Where(w => w.Id == workType).FirstOrDefault().Name);
            }
            if (region != null)
            {
                _region = (int)region;
                _name = "";
                if (categories.ContainsKey("Регион")) categories["Регион"] = db.Regions.Where(r => r.Id == region).FirstOrDefault().Name;
                else categories.Add("Регион", db.Regions.Where(r => r.Id == region).FirstOrDefault().Name);
            }
            if (salary != null)
            {
                _salary = salary;
                _name = "";
                if (categories.ContainsKey("Зарплата"))
                {
                    if(salary == 0)
                        categories["Зарплата"] = $"Указана";
                    else
                        categories["Зарплата"] = $"От {salary} т.р.";
                }
                else
                {
                    if (salary == 0)
                        categories.Add("Зарплата", $"Указана");
                    else
                        categories.Add("Зарплата", $"От {salary} т.р.");
                }
            }

            var ads1 = db.Ads
                .Include(a => a.Company)
                .Include(a => a.Region)
                .Include(a => a.WorkType)
                .Where(a => EF.Functions.Like(a.Name, $"%{_name}%") && 
                (_region != null ? a.RegionId == _region : true) && 
                (_workType != null ? a.WorkTypeId == _workType : true) && 
                (_salary != null ? (int)a.Salary > _salary*1000 : true));
            var ads2 = db.Ads
                .Include(a => a.Company)
                .Include(a => a.Region)
                .Include(a => a.WorkType)
                .Where(a => EF.Functions.Like(a.Company.Name, $"%{_name}%") &&
                (_region != null ? a.RegionId == _region : true) &&
                (_workType != null ? a.WorkTypeId == _workType : true) &&
                (_salary != null ? (int)a.Salary > _salary*1000 : true));
            var ads = ads1.Concat(ads2);

            ViewBag.currentPage = currentPage;
            ViewBag.countRegions = countRegions;

            var workTypes = db.WorkTypes.ToList();
            var regions = db.Regions.ToList();
            var model = new GetResultViewModel(ads, workTypes, regions, categories);
            return View(model);
        }

        public IActionResult AdPage(int id)
        {
            return View(db.Ads.Include(a => a.Company).Include(a => a.WorkType).Include(a => a.Region).Where(a => a.Id == id).FirstOrDefault());
        }

        public IActionResult CompanyPage(int id, int currentPage = 1)
        {
            ViewBag.currentPage = currentPage;
            var name = db.Companies.Where(c => c.Id == id).FirstOrDefault().Name;
            var img = db.Companies.Where(c => c.Id == id).FirstOrDefault().Img;
            var ads = db.Ads.Include(a => a.Company).Include(a => a.WorkType).Include(a => a.Region).Where(a => a.Company.Id == id);
            return View(new CompanyPageViewModel(name, img, ads));
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
                companies.Add(new Company() { 
                    Name = $"Компания {i + 1}",
                    Img = "https://condenast-media.gcdn.co/ad/38d357e7d51be2efdc52453e1308fa3b.jpg/e2d6f5ce/o/w1023"
                });
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
                    Company = companies[new Random().Next(0, companies.Count())],
                    Region = regions[new Random().Next(regions.Count() - 1)],
                    Date = new DateTime(2020, new Random().Next(1, 12), new Random().Next(1, 28), new Random().Next(1, 24), 0, 0),
                    Salary = new Random().Next(10000, 500000),
                    WorkType = workTypes[new Random().Next(workTypes.Count() - 1)],
                    Description = "В частности, консультация с широким активом требует определения и уточнения направлений прогрессивного развития. Разнообразный и богатый опыт говорит нам, что понимание сути ресурсосберегающих технологий предполагает независимые способы реализации распределения внутренних резервов и ресурсов. Есть над чем задуматься: явные признаки победы институционализации рассмотрены исключительно в разрезе маркетинговых и финансовых предпосылок. Учитывая ключевые сценарии поведения, убеждённость некоторых оппонентов позволяет оценить значение новых предложений. И нет сомнений, что непосредственные участники технического прогресса объявлены нарушающими общечеловеческие нормы этики и морали. Разнообразный и богатый опыт говорит нам, что синтетическое тестирование является качественно новой ступенью как самодостаточных, так и внешне зависимых концептуальных решений. Как принято считать, диаграммы связей призывают нас к новым свершениям, которые, в свою очередь, должны быть призваны к ответу. Но базовые сценарии поведения пользователей неоднозначны и будут разоблачены. Повседневная практика показывает, что перспективное планирование способствует подготовке и реализации первоочередных требований. Интерактивные прототипы освещают чрезвычайно интересные особенности картины в целом, однако конкретные выводы, разумеется, преданы социально-демократической анафеме.",
                });
            }
            db.Ads.AddRange(ads);

            db.SaveChanges();
        }
    }
}
