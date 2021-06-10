using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class TagMapper: Profile
    {
        public TagMapper()
        {
            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.Count, src => src.MapFrom(src => src.TagsTasks.Count));

            CreateMap<TagInsertOrUpdateDTO, Tag>();
        }
    }
}
