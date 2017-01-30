using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using HiddenSound.API.Areas.Application.Repositories;

namespace HiddenSound.API.Areas.Application
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().PropertiesAutowired();
        }
    }
}
