
using System.Web.Http;
using ParrotWings.App_Start;

namespace ParrotWings
{
    public class  WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityWebActivator.Start();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_End()
        {
            UnityWebActivator.Shutdown();
        }
    }
}
