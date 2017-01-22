using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using HiddenSound.API.Repositories;

namespace HiddenSound.API
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValuesRepository>().As<IValuesRepository>().PropertiesAutowired();
        }
    }
}
