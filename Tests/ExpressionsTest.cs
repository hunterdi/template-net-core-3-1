using Architecture;
using Architecture.Extensions.BuildExpression;
using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using Xunit;

namespace Tests
{
    public class ExpressionsTest
    {
        private readonly IList<Tasks> _tasksListTest = new List<Tasks>
        {
            new Tasks
            {
                Active = true,
                Priority = (int)PriorityType.High,
                Created = DateTime.Parse("31/12/2020"),
                ActivityType = (int)ActivityType.Meeting,
                TaskLists = new TaskList
                {
                    Name = "T2"
                },
                TagsTasks = new List<TagTask>
                {
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 1"
                        },
                    },
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 2"
                        },
                    }
                }
            },
            new Tasks
            {
                Active = false,
                Priority = (int)PriorityType.High,
                Created = DateTime.Parse("31/12/2020"),
                ActivityType = (int)ActivityType.Meeting,
                TaskLists = new TaskList
                {
                    Name = "T2"
                },
                TagsTasks = new List<TagTask>
                {
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 1"
                        },
                    },
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 2"
                        },
                    }
                }
            },
            new Tasks
            {
                Active = true,
                Priority = (int)PriorityType.Medium,
                Created = DateTime.Parse("31/12/2020"),
                ActivityType = (int)ActivityType.Meeting,
                TaskLists = new TaskList
                {
                    Name = "T2"
                },
                TagsTasks = new List<TagTask>
                {
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 1"
                        },
                    },
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 2"
                        },
                    }
                }
            },
            new Tasks
            {
                Active = true,
                Priority = (int)PriorityType.High,
                Created = DateTime.Parse("31/12/2021"),
                ActivityType = (int)ActivityType.Meeting,
                TaskLists = new TaskList
                {
                    Name = "T2"
                },
                TagsTasks = new List<TagTask>
                {
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 1"
                        },
                    },
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 2"
                        },
                    }
                }
            }
        };

        private readonly Tasks _tasksTest = new Tasks
        {
            Active = true,
            Priority = (int)PriorityType.High,
            Created = DateTime.Parse("31/12/2020"),
            ActivityType = (int)ActivityType.Meeting,
            TaskLists = new TaskList
            {
                Name = "T2"
            },
            TagsTasks = new List<TagTask>
                {
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 1"
                        },
                    },
                    new TagTask
                    {
                        Tag = new Tag
                        {
                            Name = "Tag 2"
                        },
                    }
                }
        };

        [Fact]
        public void Should_be_possible_get_properties_of_object()
        {
            var obj = this._tasksTest;

            var props = obj.GetProperties();

            Assert.True(props.Count > 0);
        }

        [Fact]
        public void Should_be_possible_get_properties_of_object_not_null()
        {
            var obj = this._tasksTest;

            var props = obj.GetPropertiesNotNull();

            Assert.True(props.Count > 0);
        }

        [Fact]
        public void Should_be_possible_set_build_where()
        {
            var obj = this._tasksTest;
            var props = obj.GetPropertiesNotNull();

            Expression<Func<Tasks, bool>> where = null;

            foreach (var prop in props)
            {
                if (where == null)
                {
                    //where = ExpressionHelper.GetCriteriaWhere<Tasks>(prop.Key, OperationExpression.Equals, prop.Value);
                }
                else
                {
                    //where.And(ExpressionHelper.GetCriteriaWhere<Tasks>(prop.Key, OperationExpression.Equals, prop.Value));
                }
            }

            Assert.True(where != null);
        }

        [Fact]
        public void Should_be_possible_filter()
        {
            var obj = this._tasksTest;
            var props = obj.GetPropertiesNotNull();

            Expression<Func<Tasks, bool>> where = null;

            foreach (var prop in props)
            {
                if (where == null)
                {
                    //where = ExpressionHelper.GetCriteriaWhere<Tasks>(prop.Key, OperationExpression.Equals, prop.Value);
                }
                else
                {
                    //where.And(ExpressionHelper.GetCriteriaWhere<Tasks>(prop.Key, OperationExpression.Equals, prop.Value));
                }
            }

            var result = _tasksListTest.AsQueryable().Where(where).ToList();

            Assert.True(result.Count > 0);
        }

        //[Fact]
        //public void Properties_Test()
        //{
        //    Type t = this._tasksTest.GetType();

        //    Console.WriteLine("Methods:");
        //    System.Reflection.MethodInfo[] methodInfo = t.GetMethods();

        //    foreach (System.Reflection.MethodInfo mInfo in methodInfo)
        //        Console.WriteLine(mInfo.ToString());

        //    Console.WriteLine("Members:");
        //    System.Reflection.MemberInfo[] memberInfo = t.GetMembers();

        //    foreach (System.Reflection.MemberInfo mInfo in memberInfo)
        //        Console.WriteLine(mInfo.ToString());
        //}
    }
}
