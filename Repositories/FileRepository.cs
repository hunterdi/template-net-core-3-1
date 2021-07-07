using Architecture;
using Business.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class FileRepository : RepositoryBase<File, DbContext>, IFileRepository
    {
        public FileRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
