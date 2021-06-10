using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TaskInsertOrUpdateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public DateTime RemindMeOn { get; set; }
        public string ActivityType { get; set; }
        public string Status { get; set; }
        public Guid TaskListId { get; set; }
        public IEnumerable<TagInsertOrUpdateDTO> Tags { get; set; }
    }
}
