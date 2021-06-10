using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TaskList: BaseDomain
    {
        public string Name { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
