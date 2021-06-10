using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public abstract class ServiceBase<TDomain> : IServiceBase<TDomain> where TDomain : BaseDomain
	{
		protected readonly IRepositoryBase<TDomain> _repositoryBase;

		public ServiceBase(IRepositoryBase<TDomain> repositoryBase)
		{
			this._repositoryBase = repositoryBase;
		}

		public TDomain Get(Expression<Func<TDomain, bool>> match)
		{
			throw new NotImplementedException();
		}

		public ICollection<TDomain> GetAll(Expression<Func<TDomain, bool>> match)
		{
			throw new NotImplementedException();
		}

		public virtual Task<ICollection<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>> match)
		{
			throw new NotImplementedException();
		}

		public Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> match)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TDomain> GetBy(Expression<Func<TDomain, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<TDomain>> GetByAsync(Expression<Func<TDomain, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<ICollection<TDomain>> GetAllByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true)
		{
			return await this._repositoryBase.GetAllByAsync(match, asNoTracking);
		}

		public IQueryable<TDomain> GetAll()
		{
			throw new NotImplementedException();
		}

		public virtual async Task<ICollection<TDomain>> GetAllAsync()
		{
			return await this._repositoryBase.GetAllAsync();
		}

		public IQueryable<TDomain> GetAllIncluding(params Expression<Func<TDomain, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<TDomain>> GetAllIncludingAsync(params Expression<Func<TDomain, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<ICollection<TDomain>> GetByIncludingAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true,
			params Expression<Func<TDomain, object>>[] includeProperties)
		{
			return await this._repositoryBase.GetByIncludingAsync(match, asNoTracking, includeProperties);
		}

		public TDomain GetById(Guid id)
		{
			return this._repositoryBase.GetById(id);
		}

		public virtual async Task<TDomain> GetByIdAsync(Guid id)
		{
			return await this._repositoryBase.GetByIdAsync(id);
		}

		public virtual TDomain Create(TDomain obj)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<TDomain> CreateAsync(TDomain obj)
		{
			var result = await this._repositoryBase.CreateAsync(obj);
			await this._repositoryBase.SaveChangesAsync();

			return result;
		}

		public virtual IEnumerable<TDomain> CreateCollectionWithProxy(IEnumerable<TDomain> domains)
		{
			return this._repositoryBase.CreateWithProxy(domains);
		}

		public virtual async Task<ICollection<TDomain>> CreateCollectionAsync(ICollection<TDomain> domains)
		{
			var result = await this._repositoryBase.CreateAsync(domains);
			await this._repositoryBase.SaveChangesAsync();

			return result;
		}

		public virtual TDomain Update(Guid id, TDomain obj)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<TDomain> UpdateAsync(Guid id, TDomain obj)
		{
			await this._repositoryBase.UpdateAsync(id, obj);
			await this._repositoryBase.SaveChangesAsync();

			return obj;
		}

		public virtual void Delete(Guid id)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<TDomain> DeleteAsync(Guid id)
		{
			var result = await this._repositoryBase.DeleteByIdAsync(id);
			await this._repositoryBase.SaveChangesAsync();
			return result;
		}

        public virtual async Task<ICollection<TDomain>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
			return await this._repositoryBase.GetByIdsAsync(ids);
        }
    }
}
