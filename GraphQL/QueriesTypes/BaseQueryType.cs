using Business;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL
{
    public abstract class BaseQueryType<TDomain>: ObjectType<BaseQuery<TDomain>> where TDomain: BaseDomain
    {
        protected override void Configure(IObjectTypeDescriptor<BaseQuery<TDomain>> descriptor)
        {
            base.Configure(descriptor);

            //descriptor.Name($"{typeof(TDomain).Name}Response");
            descriptor.Field(f => f.GetAllAsync(default));
        }
    }
}
