using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Repositories;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace HiddenSound.API
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<Areas.Application.Module>();

            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>().PropertiesAutowired();

            builder.RegisterType<QRService>().As<IQRService>().PropertiesAutowired();

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
