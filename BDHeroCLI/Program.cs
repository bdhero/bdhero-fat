using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero;
using BDHero.Config;

namespace BDHeroCLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            var cli = new CLI(NinjectFactory.CreateMainKernel());
            cli.Run(args);
        }
    }
}
