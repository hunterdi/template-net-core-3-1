using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TagInsertOrUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TaskId { get; set; }
    }
}
