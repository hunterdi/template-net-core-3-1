using Architecture;
using Business;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class TagService : ServiceBase<Tag>, ITagService
    {
        public TagService(ITagRepository repositoryBase) : base(repositoryBase)
        {
        }
    }
}
