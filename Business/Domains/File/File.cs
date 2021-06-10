using Business.Anotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Domains
{
    public class File: BaseDomain
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string HashName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileBytes { get; set; }
        public Guid AccountID { get; set; }
    }
}
