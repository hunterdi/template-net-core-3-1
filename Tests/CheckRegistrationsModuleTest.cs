using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class CheckRegistrationsModuleTest
    {
        [Fact]
        public void Should_Have_Register_Service_Types()
        {
            var typesToCheck = new List<Type>
            {
                typeof (ILifetimeScope),
                typeof (IComponentContext),
                typeof (ITaskListService),
                typeof (ITagService),
                typeof (ITaskService)
            };

            var typesRegistered = this.GetTypesRegisteredInModule(new ServicesModule());

            Assert.Equal(typesToCheck.Count, typesRegistered.Count());

            foreach (var typeToCheck in typesToCheck)
            {
                Assert.True(typesRegistered.Any(x => x == typeToCheck), typeToCheck.Name + " was not found in module");
            }
        }

        private IEnumerable<Type> GetTypesRegisteredInModule(Module module)
        {
            var builder = new ContainerBuilder();
            IContainer container = builder.Build();

            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                IComponentRegistry componentRegistry = scope.ComponentRegistry;
                module.Configure(builder.ComponentRegistryBuilder);

                var typesRegistered =
                    componentRegistry.Registrations.SelectMany(x => x.Services)
                        .Cast<TypedService>()
                        .Select(x => x.ServiceType);
                return typesRegistered;
            }
        }
    }
}
