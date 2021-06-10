using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class Tasks: BaseDomain
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public DateTime RemindMeOn { get; set; }
        public int ActivityType { get; set; }
        public string Status { get; set; }
        public Guid TaskListId { get; set; }
        public virtual TaskList TaskLists { get; set; }
        public virtual ICollection<TagTask> TagsTasks { get; set; }
    }

    public enum ActivityType : int
    {
        Meeting, 
        Free,
    }

    public enum PriorityType: int
    {
        High,
        Medium,
        Low
    }
}
