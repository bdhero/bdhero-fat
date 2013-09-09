using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Plugin;
using BDHero.Startup;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Updater;
using log4net;

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
            Bind<UpdaterClient>().ToSelf().InSingletonScope();
            Bind<ILog>().ToMethod(CreateLogger);
        }

        private static ILog CreateLogger(IContext context)
        {
            return LogManager.GetLogger(context.Request.Target.Type);
        }

        private void BindMainDependencies()
        {
            Bind<IController>().To<Controller>();
        }
    }
}
