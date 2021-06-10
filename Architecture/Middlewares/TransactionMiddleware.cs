using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationDataContext _dataContext;
        private IDbContextTransaction _transaction { get; set; }

        public TransactionMiddleware(RequestDelegate next, ApplicationDataContext dataContext)
        {
            this._next = next;
            this._dataContext = dataContext;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "DELETE")
            {
                await TransactionDatabaseAsync(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task TransactionDatabaseAsync(HttpContext context)
        {
            this._transaction = await this._dataContext.Database.BeginTransactionAsync();
            await _next.Invoke(context);
            await this._transaction.CommitAsync();
        }
    }
}
