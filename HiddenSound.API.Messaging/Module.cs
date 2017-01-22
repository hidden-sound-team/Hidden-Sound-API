using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace HiddenSound.API.Messaging
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailService>().As<IEmailService>().PropertiesAutowired();
            builder.RegisterType<EmailWebFactory>().As<IEmailWebFactory>().PropertiesAutowired();
        }
    }
}
