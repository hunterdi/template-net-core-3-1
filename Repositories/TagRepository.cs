using Architecture;
using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class TagRepository : RepositoryBase<Tag, DbContext>, ITagRepository
    {
        public TagRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
