using NotHhru.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotHhru.ViewModels;

namespace NotHhru.Components
{
    public class AdListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IQueryable<Ad> ads, int pageSize, int pageicoSize, int currentPage, string controller, string action)
        {
            int count = ads.Count();
            var page = new PageViewModel<Ad>(count, pageSize, currentPage, ads);
            ViewBag.action = action;
            ViewBag.controller = controller;
            ViewBag.pageicoSize = pageicoSize;
            return View(page);
        }

    }
}
