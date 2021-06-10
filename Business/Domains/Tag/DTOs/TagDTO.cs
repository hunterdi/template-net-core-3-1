using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TagDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Guid TaskId { get; set; }
    }
}
