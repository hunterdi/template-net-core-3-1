using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class PagedList<TDomain>: List<TDomain> where TDomain: BaseDomain
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList(List<TDomain> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static PagedList<TDomain> ToPagedList(IQueryable<TDomain> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<TDomain>(items, count, pageNumber, pageSize);
        }
    }
}


//PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
//        ownerParameters.PageNumber,
//        ownerParameters.PageSize);

//var metadata = new
//{
//    owners.TotalCount,
//    owners.PageSize,
//    owners.CurrentPage,
//    owners.TotalPages,
//    owners.HasNext,
//    owners.HasPrevious
//};
//Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));