using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using System.Web.OData.Builder;
using ODataDemo.Models;

namespace ODataDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("DefaultRoute", "api", GetEdmModel());
            config.EnsureInitialized();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "OdataDemo";
            builder.ContainerName = "OdataDemoContainer";

            builder.EntitySet<Person>("People");

            return builder.GetEdmModel();
        }
    }
}
