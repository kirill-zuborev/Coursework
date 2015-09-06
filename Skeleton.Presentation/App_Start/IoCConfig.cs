using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using Skeleton.Domain;

namespace Skeleton.Presentation
{
	public static class IoCConfig
	{
		public  static void RegisterDependencies()
		{
			var container = new ContainerBuilder();
			container.RegisterControllers(Assembly.GetExecutingAssembly());
			container.RegisterModule(new ConfigurationSettingsReader());

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container.Build()));
		}
	}
}