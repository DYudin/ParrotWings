
using Unity.WebApi;
using System.Web.Http;

namespace ParrotWings.App_Start
{
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            UnityConfig.GetContext();
            var container = UnityConfig.GetConfiguredContainer();
            UnityConfig.RegisterComponents();                                          
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);            
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
            var context = UnityConfig.GetContext();
            context.Dispose();
        }
    }
}