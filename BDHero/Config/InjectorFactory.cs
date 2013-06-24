using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Plugin;
using BDHero.Startup;
using Ninject;
using Ninject.Modules;

namespace BDHero.Config
{
    public static class InjectorFactory
    {
        public static IKernel CreateContainer()
        {
            return new StandardKernel(new MainModule());
        }
    }

    /// <summary>
    /// Module used by main (non-test) code.
    /// </summary>
    class MainModule : NinjectModule
    {
        public override void Load()
        {
            BindStartupDependencies();
            BindMainDependencies();
        }

        private void BindStartupDependencies()
        {
            Bind<IDirectoryLocator>().To<DirectoryLocator>().InSingletonScope();
            Bind<LogInitializer>().ToSelf().InSingletonScope();
            Bind<PluginService>().ToSelf().InSingletonScope();
            Bind<PluginLoader>().ToSelf().InSingletonScope();
        }

        private void BindMainDependencies()
        {
            Bind<IController>().To<Controller>();
        }
    }
}
