using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.Config;
using BDHero.Startup;
using Ninject;

namespace BDHeroCLI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-cli.log.config";

        static void Main(string[] args)
        {
            var kernel = InjectorFactory.CreateContainer();
            kernel.Get<LogInitializer>().Initialize(LogConfigFileName);
            kernel.Bind<CLI>().ToSelf();
            kernel.Get<CLI>().Run(args);
        }
    }
}
