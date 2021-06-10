using Architecture;
using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class TaskListRepository : RepositoryBase<TaskList, DbContext>, ITaskListRepository
    {
        public TaskListRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
