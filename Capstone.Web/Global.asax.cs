﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Capstone.Web.DAL;
using Ninject;
using Ninject.Web.Common.WebHost;

namespace Capstone.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            kernel.Bind<IParksWeatherDAL>().To<ParksWeatherSqlDAL>().WithConstructorArgument("connectionString", connectionString);

            return kernel;
        }
    }
}
