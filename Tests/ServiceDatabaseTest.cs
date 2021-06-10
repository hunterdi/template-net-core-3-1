using Architecture;
using Autofac;
using Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;
using Moq;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ServiceDatabaseTest
    {
        [Fact]
        public async Task Must_check_dbcontext_operations()
        {
            var service = this.GetService<TagService, TagRepository>();
            var result = await service.GetAllAsync();

            Assert.True(result.Count == 0);

            var obj = new Tag
            {
                Name = "Teste"
            };

            await service.CreateAsync(obj);
            result = await service.GetAllAsync();

            Assert.True(result.Count == 1);
            Assert.True(result.FirstOrDefault().Visible);
        }
        
        private TService GetService<TService, TRepository>() where TService: class where TRepository: class
        {
            var db = this.GetDbContext();

            var respository = (TRepository)Activator.CreateInstance(typeof(TRepository), db);
            var service = (TService)Activator.CreateInstance(typeof(TService), respository);

            return service;
        }

        private ApplicationDataContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>()
                .UseInMemoryDatabase("MovieCup").Options;
            var dbContext = new ApplicationDataContext(optionsBuilder);

            return dbContext;
        }
    }
}
