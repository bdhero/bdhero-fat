using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Exceptions;
using BDHero.Plugin;
using BDHero.Startup;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;

namespace BDHero.Config
{
    public static class NinjectFactory
    {
        public static IKernel CreateMainKernel()
        {
            return new StandardKernel(new MainModule());
        }

        private static void InitializeCriticalDependencies(IKernel kernel)
        {
            // NOTE: The order of instantiation is EXTREMELY important:
            // the DirectoryLocator MUST be initialized FIRST;
            // the LogInitializer MUST be initialized SECOND;
            // the PluginService and PluginLoader must be initialized LAST.
//            var directoryLocator = DirectoryLocator.Instance;
//            var logInitializer = new LogInitializer(logConfigFileName, directoryLocator);
//            var pluginService = new PluginService();
//            var pluginLoader = new PluginLoader(pluginService, directoryLocator);
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
