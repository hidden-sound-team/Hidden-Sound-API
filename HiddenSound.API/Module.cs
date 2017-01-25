using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using HiddenSound.API.Repositories;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace HiddenSound.API
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValuesRepository>().As<IValuesRepository>().PropertiesAutowired();
            builder.RegisterType<APIKeyRepository>().As<IAPIKeyRepository>().PropertiesAutowired();

            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId).Where(l => l.Name.StartsWith($"{GetType().Namespace}."));
            foreach (var assemblyName in assemblies)
            {
                var assembly = Assembly.Load(new AssemblyName(assemblyName.Name));
                var modules = assembly.ExportedTypes.Where(t => t.IsAssignableTo<IModule>()).ToList();
                foreach (var module in modules)
                {
                    var instance = (IModule) Activator.CreateInstance(module);
                    builder.RegisterModule(instance);
                }
            }
        }
    }
}
