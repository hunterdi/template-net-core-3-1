using Architecture;
using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class AccountRepository: RepositoryBase<Account, DbContext>, IAccountRepository
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
