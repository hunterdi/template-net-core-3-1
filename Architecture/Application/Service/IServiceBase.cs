using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture
{
    public interface IServiceBase<TDomain> where TDomain : BaseDomain
	{
		TDomain Create(TDomain obj);
		Task<TDomain> CreateAsync(TDomain obj);
		IEnumerable<TDomain> CreateCollectionWithProxy(IEnumerable<TDomain> domains);
		Task<ICollection<TDomain>> CreateCollectionAsync(ICollection<TDomain> domains);
		void Delete(Guid id);
		Task<TDomain> DeleteAsync(Guid id);
		TDomain Get(Expression<Func<TDomain, bool>> match);
		Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> match);
		ICollection<TDomain> GetAll(Expression<Func<TDomain, bool>> match);
		Task<ICollection<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>> match);
		IQueryable<TDomain> GetBy(Expression<Func<TDomain, bool>> predicate);
		Task<ICollection<TDomain>> GetByAsync(Expression<Func<TDomain, bool>> predicate);
		TDomain GetById(Guid id);
		Task<TDomain> GetByIdAsync(Guid id);
		IQueryable<TDomain> GetAll();
		Task<ICollection<TDomain>> GetAllAsync();
		Task<ICollection<TDomain>> GetAllByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true);
		IQueryable<TDomain> GetAllIncluding(params Expression<Func<TDomain, object>>[] includeProperties);
		Task<ICollection<TDomain>> GetAllIncludingAsync(params Expression<Func<TDomain, object>>[] includeProperties);
		Task<ICollection<TDomain>> GetByIncludingAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true, params Expression<Func<TDomain, object>>[] includeProperties);
		TDomain Update(Guid id, TDomain obj);
		Task<TDomain> UpdateAsync(Guid id, TDomain obj);
		Task<ICollection<TDomain>> GetByIdsAsync(IEnumerable<Guid> ids);
	}
}
