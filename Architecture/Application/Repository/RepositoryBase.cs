using Business;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    public abstract class RepositoryBase<TDomain, TContext> : IRepositoryBase<TDomain>
        where TDomain : BaseDomain where TContext : DbContext, IDisposable
    {
        protected readonly TContext _dbContext;
        protected readonly DbSet<TDomain> _entitySet;
        private bool disposed = false;

        public RepositoryBase(TContext dbContext)
        {
            this._dbContext = dbContext;
            this._entitySet = this._dbContext.Set<TDomain>();
        }

        public IQueryable<TDomain> Pagination(Expression<Func<TDomain, bool>> filter, Expression<Func<TDomain, object>> orderBy, int pageSize, int pageIndex)
        {
            return this._entitySet.Where(filter).OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }

        public virtual async Task<Tuple<ICollection<TDomain>, int>> GetAllAsync(int skip, int take, Expression<Func<TDomain, bool>> where,
            Expression<Func<TDomain, object>> orderBy, bool asNoTracking = true)
        {
            var dataBaseCount = await this._entitySet.CountAsync();
            if (asNoTracking)
            {
                return new Tuple<ICollection<TDomain>, int>(await this._entitySet.AsNoTracking().Where(where).OrderBy(orderBy).Skip(skip).Take(take).ToListAsync(), dataBaseCount);
            }

            return new Tuple<ICollection<TDomain>, int>(await this._entitySet.Where(where).OrderBy(orderBy).Skip(skip).Take(take).ToListAsync(), dataBaseCount);
        }

        public virtual async Task<ICollection<TDomain>> GetAllByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true)
        {
            if (asNoTracking)
                return await this._entitySet.AsNoTracking().Where(match).ToListAsync();
            return await this._entitySet.Where(match).ToListAsync();
        }

        public virtual IQueryable<TDomain> GetAllIncluding(bool asNoTracking = true, params Expression<Func<TDomain, object>>[] includeProperties)
        {
            IQueryable<TDomain> queryable = GetAll(asNoTracking);
            foreach (Expression<Func<TDomain, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TDomain, object>(includeProperty);
            }

            return queryable;
        }

        public virtual IQueryable<TDomain> GetByIncluding(Expression<Func<TDomain, bool>> match, bool asNoTracking = true,
            params Expression<Func<TDomain, object>>[] includeProperties)
        {
            IQueryable<TDomain> queryable = GetAll(asNoTracking);
            foreach (Expression<Func<TDomain, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TDomain, object>(includeProperty);
            }

            return queryable.Where(match).Where(e => e.Visible == true);
        }

        public virtual async Task<ICollection<TDomain>> GetAllIncludingAsync(bool asNoTracking = true, params Expression<Func<TDomain, object>>[] includeProperties)
        {
            return await GetAllIncluding(asNoTracking, includeProperties).ToListAsync();
        }

        public virtual async Task<ICollection<TDomain>> GetByIncludingAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true,
            params Expression<Func<TDomain, object>>[] includeProperties)
        {
            return await GetByIncluding(match, asNoTracking, includeProperties).ToListAsync();
        }

        public virtual IQueryable<TDomain> GetAll(bool asNoTracking = true)
        {
            if (asNoTracking)
                return this._entitySet.AsNoTracking();

            return this._entitySet;
        }

        public virtual async Task<ICollection<TDomain>> GetAllAsync(bool asNoTracking = true)
        {
            var result = this._entitySet.AsQueryable<TDomain>();

            if (asNoTracking)
            {
                result = this._entitySet.AsNoTracking();
            }

            result = result.Where(e => e.Visible == true);

            return await result.ToListAsync();
        }

        public virtual async Task<TDomain> GetByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true)
        {
            if (asNoTracking)
                return await this._entitySet.AsNoTracking().FirstOrDefaultAsync(match);

            return await this._entitySet.FirstOrDefaultAsync(match);
        }

        public virtual async Task<TDomain> GetByIdAsync(Guid id)
        {
            var result = await this._dbContext.FindAsync<TDomain>(id);
            if (result != null)
            {
                result = result.Visible ? result : null;
            }
            return result;
        }

        public virtual TDomain GetById(Guid id)
        {
            return this._entitySet.Find(id);
        }

        public virtual async Task<TDomain> CreateAsync(TDomain domain)
        {
            await this._entitySet.AddAsync(domain);

            return domain;
        }

        public virtual void Create(TDomain domain)
        {
            this._dbContext.Add(domain);
        }

        public virtual async Task<ICollection<TDomain>> CreateAsync(ICollection<TDomain> domains)
        {
            await this._entitySet.AddRangeAsync(domains);

            return domains;
        }

        public virtual IEnumerable<TDomain> CreateWithProxy(IEnumerable<TDomain> domains)
        {
            foreach (var domain in domains)
            {
                this._entitySet.Add(domain).State = EntityState.Added;
                yield return domain;
            }
        }

        public virtual async Task<TDomain> UpdateAsync(Guid id, TDomain domain)
        {
            var entity = await this.GetByIdAsync(id);

            this._dbContext.Entry(entity).CurrentValues.SetValues(domain);
            this._entitySet.Update(entity).State = EntityState.Modified;

            return entity;
        }

        public virtual Task UpdateAsync(ICollection<TDomain> domains)
        {
            this._entitySet.UpdateRange(domains);
            return Task.CompletedTask;
        }

        public virtual IEnumerable<TDomain> UpdateWithProxy(IEnumerable<TDomain> domains)
        {
            foreach (var domain in domains)
            {
                this._entitySet.Update(domain).State = EntityState.Modified;
                yield return domain;
            }
        }

        public virtual Task DeleteByAsync(Func<TDomain, bool> where)
        {
            this._entitySet.RemoveRange(this._entitySet.Where(where));
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TDomain domain)
        {
            this._entitySet.Remove(domain).State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public virtual void Delete(Func<TDomain, bool> predicate)
        {
            this._entitySet
                .Where(predicate).ToList()
                .ForEach(del => this._dbContext.Set<TDomain>().Remove(del));
        }

        public virtual async Task<TDomain> DeleteByIdAsync(Guid id)
        {
            TDomain entity = await this._entitySet.FindAsync(id);

            if (entity != null)
            {
                this._entitySet.Remove(entity).State = EntityState.Deleted;
            }
            return entity;
        }

        public virtual async Task SaveChangesAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public virtual async Task<ICollection<TDomain>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var result = this._entitySet.Where(e => ids.Contains(e.Id));

            return await result.ToListAsync();
        }

        public virtual async Task BulkMergeAsync(params Expression<Func<TDomain, object>>[] changes)
        {
            await this._dbContext.BulkMergeAsync(changes);
        }

        public virtual async Task BulkInsertAsync(IEnumerable<TDomain> domains)
        {
            await this._dbContext.BulkInsertAsync(domains);
        }

        public virtual async Task BulkUpdateAsync(IEnumerable<TDomain> domains)
        {
            await this._dbContext.BulkUpdateAsync(domains);
        }

        public virtual async Task BulkDeleteAsync(IEnumerable<TDomain> domains)
        {
            await this._dbContext.BulkDeleteAsync(domains);
        }

        public virtual async Task BulkSaveChangesAsync()
        {
            await this._dbContext.BulkSaveChangesAsync();
        }
    }
}
