using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class TaskMapper: Profile
    {
        public TaskMapper()
        {
            CreateMap<Tasks, TaskDTO>()
                .AfterMap((src, dest, ctx) =>
                {
                    if (src.TaskLists != null)
                    {
                        dest.TaskLists = ctx.Mapper.Map<TaskListDTO>(src.TaskLists);
                    }
                    if (src.TagsTasks != null)
                    {
                        dest.Tags = ctx.Mapper.Map<IEnumerable<TagDTO>>(src.TagsTasks.Select(e => e.Tag));
                    }
                });

            CreateMap<TaskInsertOrUpdateDTO, Tasks>()
                .AfterMap((src, dest, ctx) => {
                    dest.TagsTasks = ctx.Mapper.Map<ICollection<TagTask>>(src.Tags);
                });
        }
    }
}
