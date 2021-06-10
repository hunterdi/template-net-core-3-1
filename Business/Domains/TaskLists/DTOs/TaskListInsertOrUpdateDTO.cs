using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TaskListInsertOrUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskInsertOrUpdateDTO> Tasks { get; set; }
    }
}
