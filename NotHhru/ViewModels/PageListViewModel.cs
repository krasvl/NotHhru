using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.ViewModels
{
    public class PageListViewModel
    {
        public string Controller { get; set; } 
        public string Action { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }

        public PageListViewModel(string controller, string action, int currentPage, int lastPage)
        {
            Controller = controller;
            Action = action;
            CurrentPage = currentPage;
            LastPage = lastPage;
        }
        
        public bool IsSelect(int page)
        {
            if (page == CurrentPage) return true;
            else return false;
        }
    }
}
