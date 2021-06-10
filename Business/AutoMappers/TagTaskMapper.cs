using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TagTaskMapper: Profile
    {
        public TagTaskMapper()
        {
            CreateMap<TagInsertOrUpdateDTO, TagTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(e => e.Id))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
                .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src));

            CreateMap<TagTask, TagDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => e.Tag.Id))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(e => e.TaskId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(e => e.Tag.Name));
        }
    }
}
