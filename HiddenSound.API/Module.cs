using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using HiddenSound.API.Areas.Application.Services;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Repositories;
using HiddenSound.API.OpenIddict;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace HiddenSound.API
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationRepository>().As<IAuthorizationRepository>().PropertiesAutowired();
            builder.RegisterType<DeviceRepository>().As<IDeviceRepository>().PropertiesAutowired();
            builder.RegisterType<ApplicationRepository>().As<IApplicationRepository>().PropertiesAutowired();

            builder.RegisterType<QRService>().As<IQRService>().PropertiesAutowired();
            builder.RegisterType<AuthEmailSender>().As<IEmailSender>().PropertiesAutowired();
        }
    }
}
