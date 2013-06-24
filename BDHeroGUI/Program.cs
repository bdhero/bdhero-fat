﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BDHero.Config;
using BDHero.Startup;
using Ninject;

namespace BDHeroGUI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-gui.log.config";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(CreateMainForm());
        }

        private static FormAsyncControllerTest CreateMainForm()
        {
            var kernel = InjectorFactory.CreateContainer();
            kernel.Get<LogInitializer>().Initialize(LogConfigFileName);
            kernel.Bind<FormAsyncControllerTest>().ToSelf();
            return kernel.Get<FormAsyncControllerTest>();
        }
    }
}
