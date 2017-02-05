
using Unity.WebApi;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace ParrotWings.App_Start
{
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            //UnityConfig.GetContext();
            var container = UnityConfig.GetConfiguredContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            //DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
            //var context = UnityConfig.GetContext();
            //context.Dispose();
        }
    }
}