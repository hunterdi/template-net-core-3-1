using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class QueryParameters
    {
        private int _maxPageSize = 100;
        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize 
        {
            get 
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }
    }
}


//return FindAll()
//    .OrderBy(on => on.Name)
//    .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
//    .Take(ownerParameters.PageSize)
//    .ToList();