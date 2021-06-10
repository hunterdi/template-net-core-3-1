using Business;
using Business.BaseDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Extensions.BuildExpression
{
    //https://rafaeldossantos.net/entity-framework-gerando-filtros-dinamicos/
    //PredicateExpressionExtensions
    public class GetOrdersRequest
    {
        public string OrderType { get; set; }
        public Guid? PartyId { get; set; }
        public bool? HasParty { get; set; }

        public Expression<Func<T, bool>> BuildFilter<T>() where T: IFilter
        {
            Expression<Func<T, bool>> result = order => true;

            if (!string.IsNullOrWhiteSpace(OrderType))
            {
                var orderTypesFilter = OrderType.Split(",", StringSplitOptions.RemoveEmptyEntries);
                var orderTypes = new List<int>();
                foreach (var strOrderType in orderTypesFilter)
                {
                    if (int.TryParse(strOrderType, out int orderType))
                    {
                        orderTypes.Add(orderType);
                    }
                }

                Expression<Func<T, bool>> orderTypesExpression = order => orderTypes.Contains(order.OrderType);
                //result = result.And(orderTypesExpression);
            }

            if (HasParty.HasValue)
            {
                if (HasParty.GetValueOrDefault())
                {
                    Expression<Func<T, bool>> partyFilter = o => o.ID.HasValue;
                    //result = result.And(partyFilter);
                }
                else
                {
                    Expression<Func<T, bool>> partyFilter = o => o.ID.HasValue;
                    //result = result.And(partyFilter);
                }
            }

            if (PartyId.HasValue)
            {
                Expression<Func<T, bool>> partyFilter = o => o.ID == PartyId.Value;
                //result = result.And(partyFilter);
            }

            return result;
        }
    }
}
