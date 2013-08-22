using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.Config;
using BDHero.Startup;
using BDHeroCLI.Properties;
using Ninject;

namespace BDHeroCLI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-cli.log.config";

        static void Main(string[] args)
        {
            CreateCLI().Run(args);
        }

        private static CLI CreateCLI()
        {
            var kernel = InjectorFactory.CreateContainer();
            kernel.Get<LogInitializer>().Initialize(LogConfigFileName, Resources.log4net_config);
            kernel.Bind<CLI>().ToSelf();
            return kernel.Get<CLI>();
        }
    }
}
