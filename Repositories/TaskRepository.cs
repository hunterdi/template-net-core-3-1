using Architecture;
using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class TaskRepository: RepositoryBase<Tasks, DbContext>, ITaskRepository
    {
        public TaskRepository(DbContext dataContext): base(dataContext)
        {
        }
    }
}
