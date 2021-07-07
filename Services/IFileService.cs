using Architecture;
using Business.Domains;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IFileService: IServiceBase<File>
    {
        Task<ICollection<Business.Domains.File>> CreateAsync(IFormFileCollection files, string path, Guid accountID);
    }
}
