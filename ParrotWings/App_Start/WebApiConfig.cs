﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ParrotWings.Filters;

namespace ParrotWings
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new CustomExceptionAttribute());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
