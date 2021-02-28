using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.ViewModels
{
    public class PageViewModel<T>
    {
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public int PageSize { get; set; }
        public IQueryable<T> Elements { get; set; }

        public PageViewModel(int countElements, int pageSize, int curentPage, IQueryable<T> elements)
        {
            this.PageSize = pageSize;
            CurrentPage = curentPage;
            LastPage = (int)Math.Ceiling(countElements / (double)pageSize);
            Elements = elements;
        }

        public bool IsSelect(int page)
        {
            if (page == CurrentPage) return true;
            else return false;
        }
        

        public List<T> GetElements()
        {
            return Elements.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
