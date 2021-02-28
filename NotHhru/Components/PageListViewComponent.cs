using NotHhru.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.Components
{
    public class PageListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string controller, string action, int currentPage, int lastPage)
        {
            var pageList = new PageListViewModel(controller, action, currentPage, lastPage);
            return View(pageList);
        }
    }
}
