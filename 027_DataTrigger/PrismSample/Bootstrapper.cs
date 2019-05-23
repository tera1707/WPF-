using Microsoft.Practices.Unity;
using PrismSample.Views;
using Prism.Unity;
using System.Linq;
using System.Windows;
using System;

namespace PrismSample
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            ((Window)this.Shell).Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // ViewをDIコンテナに登録(！！！すべて「Object型」で登録すること！！！(なぜかは不明))
            this.Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(x => x.Namespace.EndsWith(".Views")),
                getFromTypes: _ => new[] { typeof(object) },
                getName: WithName.TypeName);
        }
    }
}
