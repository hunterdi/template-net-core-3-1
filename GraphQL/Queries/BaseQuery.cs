using Architecture;
using Business;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL
{
    public abstract class BaseQuery<TDomain> where TDomain: BaseDomain
    {
        [UsePaging]
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public async Task<ICollection<TDomain>> GetAllAsync([Service] IServiceBase<TDomain> context) => await context.GetAllAsync();
    }
}
