using Business;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business
{
    public class RefreshToken: BaseDomain
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public override bool Active { get => this.IsActive; set => base.Active = value; }
        public bool IsActive => Revoked == null && !IsExpired;

    }
}
