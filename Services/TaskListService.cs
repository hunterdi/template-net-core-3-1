using Architecture;
using Business;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class TaskListService : ServiceBase<TaskList>, ITaskListService
    {
        public TaskListService(ITaskListRepository repositoryBase) : base(repositoryBase)
        {
        }
    }
}
