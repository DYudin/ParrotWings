using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.App_Start
{
    public class UnityPerRequestHttpModule : IHttpModule
    {
        private static readonly object ModuleKey = new object();

        public void Dispose()
        {

        }

        private static Dictionary<object, object> GetDictionary(HttpContext context)
        {
            return (Dictionary<object, object>)context.Items[ModuleKey];
        }

        internal static object GetValue(object lifetimeManagerKey)
        {
            Dictionary<object, object> dictionary = GetDictionary(HttpContext.Current);
            if (dictionary != null)
            {
                object obj2 = null;
                if (dictionary.TryGetValue(lifetimeManagerKey, out obj2))
                {
                    return obj2;
                }
            }
            return null;
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(this.OnEndRequest);
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            Dictionary<object, object> dictionary = GetDictionary(application.Context);
            if (dictionary != null)
            {
                foreach (IDisposable disposable in dictionary.Values.OfType<IDisposable>())
                {
                    disposable.Dispose();
                }
            }
        }

        internal static void SetValue(object lifetimeManagerKey, object value)
        {
            Dictionary<object, object> dictionary = GetDictionary(HttpContext.Current);
            if (dictionary == null)
            {
                dictionary = new Dictionary<object, object>();
                HttpContext.Current.Items[ModuleKey] = dictionary;
            }
            dictionary[lifetimeManagerKey] = value;
        }
    }
}