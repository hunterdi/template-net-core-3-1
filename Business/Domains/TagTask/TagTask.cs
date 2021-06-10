using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TagTask: BaseDomain
    {
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public Guid TaskId { get; set; }
        public virtual Tasks Task { get; set; }
    }
}
