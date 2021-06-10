using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class Tag: BaseDomain
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public virtual ICollection<TagTask> TagsTasks { get; set; }
    }
}
