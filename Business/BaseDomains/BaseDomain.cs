using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public abstract class BaseDomain: BaseDomain<Guid>
    {
    }

    public abstract class BaseDomain<TPK> where TPK: IComparable
    {
        public virtual TPK Id { get; set; }
        public virtual bool Active { get; set; } = true;
        public virtual bool Visible { get; set; } = true;
        public virtual DateTime Created { get; set; }
        public virtual DateTime? Updated { get; set; }
        public virtual DateTime? Deleted { get; set; }
    }
}
