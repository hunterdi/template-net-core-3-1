using Architecture;
using Business;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class TaskService : ServiceBase<Tasks>, ITaskService
    {
        public TaskService(ITaskRepository repositoryBase) : base(repositoryBase)
        {
        }
    }
}
