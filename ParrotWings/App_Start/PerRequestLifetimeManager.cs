using System;
using Microsoft.Practices.Unity;

namespace ParrotWings.App_Start
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly object lifetimeKey = new object();

        public override object GetValue()
        {
            return UnityPerRequestHttpModule.GetValue(this.lifetimeKey);
        }

        public override void RemoveValue()
        {
            IDisposable disposable = this.GetValue() as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            UnityPerRequestHttpModule.SetValue(this.lifetimeKey, null);
        }

        public override void SetValue(object newValue)
        {
            UnityPerRequestHttpModule.SetValue(this.lifetimeKey, newValue);
        }
    }
}