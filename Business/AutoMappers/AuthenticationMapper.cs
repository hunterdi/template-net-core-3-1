using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class AuthenticationMapper : Profile
    {
        public AuthenticationMapper()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<LoginDTO, Account>();
            CreateMap<AccountInsertDTO, Account>();
            CreateMap<AccountUpdateDTO, Account>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));

        }
    }
}
