using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BaseDomains
{
    public interface IFilter
    {
        Guid? ID { get; set; }
        int OrderType { get; set; }
    }
}
