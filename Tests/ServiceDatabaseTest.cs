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
