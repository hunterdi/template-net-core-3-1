using Architecture;
using Autofac;
using Business;
using Business.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class RepositoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AccountRepository>().As<IRepositoryBase<Account>>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<FileRepository>().As<IFileRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<FileRepository>().As<IRepositoryBase<File>>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
