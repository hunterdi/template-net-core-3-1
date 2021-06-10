using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public interface IRepositoryBase<TDomain> where TDomain : BaseDomain
	{
		IQueryable<TDomain> Pagination(Expression<Func<TDomain, bool>> filter, Expression<Func<TDomain, object>> orderBy, int pageSize, int pageIndex);

		Task<Tuple<ICollection<TDomain>, int>> GetAllAsync(int skip, int take, Expression<Func<TDomain, bool>> where,
			Expression<Func<TDomain, object>> orderBy, bool asNoTracking = true);

		Task<ICollection<TDomain>> GetAllByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true);

		Task<ICollection<TDomain>> GetAllIncludingAsync(bool asNoTracking = true, params Expression<Func<TDomain, object>>[] includeProperties);

		IQueryable<TDomain> GetByIncluding(Expression<Func<TDomain, bool>> match, bool asNoTracking = true, params Expression<Func<TDomain, object>>[] includeProperties);

		Task<ICollection<TDomain>> GetByIncludingAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true,
			params Expression<Func<TDomain, object>>[] includeProperties);

		IQueryable<TDomain> GetAll(bool asNoTracking = true);

		Task<ICollection<TDomain>> GetAllAsync(bool asNoTracking = true);

		Task<TDomain> GetByAsync(Expression<Func<TDomain, bool>> match, bool asNoTracking = true);

		Task<TDomain> GetByIdAsync(Guid id);

		TDomain GetById(Guid id);

		void Create(TDomain domain);

        Task<TDomain> CreateAsync(TDomain domain);

		Task<ICollection<TDomain>> CreateAsync(ICollection<TDomain> domains);

		IEnumerable<TDomain> CreateWithProxy(IEnumerable<TDomain> domains);

        Task<TDomain> UpdateAsync(Guid id, TDomain domain);

        Task UpdateAsync(ICollection<TDomain> domains);

		IEnumerable<TDomain> UpdateWithProxy(IEnumerable<TDomain> domains);

        Task DeleteByAsync(Func<TDomain, bool> where);

        Task DeleteAsync(TDomain domain);

		Task<TDomain> DeleteByIdAsync(Guid id);

        Task SaveChangesAsync();

		void Dispose();

        Task<ICollection<TDomain>> GetByIdsAsync(IEnumerable<Guid> ids);

		Task BulkMergeAsync(params Expression<Func<TDomain, object>>[] changes);

		Task BulkInsertAsync(IEnumerable<TDomain> domains);

		Task BulkUpdateAsync(IEnumerable<TDomain> domains);

		Task BulkDeleteAsync(IEnumerable<TDomain> domains);

		Task BulkSaveChangesAsync();

	}
}
