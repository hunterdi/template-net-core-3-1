using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business
{
    public class TaskListMapper: Profile
    {
        public TaskListMapper()
        {
            CreateMap<TaskList, TaskListDTO>().ReverseMap();
            CreateMap<TaskListInsertOrUpdateDTO, TaskList>()
                .AfterMap((src, dest, ctx) => {
                    dest.Tasks = ctx.Mapper.Map<ICollection<Tasks>>(src.Tasks);
                });
        }
    }
}
