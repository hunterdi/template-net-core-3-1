using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TaskListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskDTO> Tasks { get; set; }
    }
}
