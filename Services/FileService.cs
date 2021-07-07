using Architecture;
using Architecture.Extensions;
using Business.Domains;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileService : ServiceBase<Business.Domains.File>, IFileService
    {
        public FileService(IRepositoryBase<Business.Domains.File> repositoryBase) : base(repositoryBase)
        {
        }

        public async Task<ICollection<Business.Domains.File>> CreateAsync(IFormFileCollection files, string path, Guid accountID)
        {
            var result = new List<Business.Domains.File>();

            foreach (var file in files)
            {
                var temp = new Business.Domains.File();
                temp.Size = file.Length;
                temp.ContentType = file.ContentType;
                temp.FilePath = path;
                temp.FileName = file.FileName;
                temp.HashName = Guid.NewGuid().ToString();
                temp.AccountID = accountID;

                using (Stream stream = file.OpenReadStream())
                {
                    byte[] buffer = stream.ConvertStreamToByteArray(temp.Size);
                    temp.FileBytes = buffer;

                    result.Add(temp);
                }
                //await this._repository.CreateAsync(temp);
                await this.CreateLocalFile(path, file, Guid.Parse(temp.HashName));
            }
            return result;
        }

        private async Task CreateLocalFile(string path, IFormFile file, Guid? name = null)
        {
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }

            var fileName = name.HasValue ? name.ToString() : ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();
            fileName = string.Format(@"{0}\{1}", path, fileName);

            using (FileStream fs = System.IO.File.Create(fileName))
            {
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }
        }
    }
}
