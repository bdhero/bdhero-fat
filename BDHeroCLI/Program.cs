using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero;
using BDHero.Plugin;
using BDHero.Startup;
using DotNetUtils;
using Mono.Options;
using ProcessUtils;

namespace BDHeroCLI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-cli.log.config";

        /// <summary>
        /// IMPORTANT: THIS MUST BE INSTANTIATED AND INITIALIZED FIRST TO ENABLE LOGGING!
        /// </summary>
        private static readonly Initializer Initializer;

        /// <summary>
        /// Depends on <see cref="Initializer"/> being initialized first.
        /// </summary>
        private static readonly log4net.ILog Logger;

        /// <summary>
        /// Depends on <see cref="Initializer"/> being initialized first.
        /// </summary>
        private static readonly IController Controller;

        static Program()
        {
            // IMPORTANT: This must be the absolute FIRST line of code that runs to initialize logging!
            Initializer = Initializer.GetInstance(LogConfigFileName);
            Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Controller = new Controller(Initializer.PluginService);
        }

        private static string ExeFileName
        {
            get { return Path.GetFileName(AssemblyUtils.AssemblyOrDefault().Location); }
        }

        static void Main(string[] args)
        {
            ShowVersion();

            bool? replace;
            string bdromPath = "",
                   mkvPath = "";

            var optionSet = new OptionSet
                {
                    { "h|?|help", v => ShowHelp() },
                    { "-V|version", s => Environment.Exit(0) },
                    { "debug", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Debug },
                    { "v|verbose", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Info },
                    { "w|warn", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Warn },
                    { "q|quiet", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Error },
                    { "s|silent", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Fatal },
                    { "i=|input=", s => bdromPath = s },
                    { "o=|output=", s => mkvPath = s },
                    { "y|yes", s => replace = true },
                    { "n|no", s => replace = false }
                };

            var extraArgs = optionSet.Parse(args);

            LogDirectoryPaths();

            if (extraArgs.Any())
            {
                Logger.WarnFormat("Unknown argument{0}: {1}", extraArgs.Count == 1 ? "" : "s", new ArgumentList(extraArgs));
            }

            // Prompt user for path to BD-ROM directory
            while (string.IsNullOrWhiteSpace(bdromPath) || !Directory.Exists(bdromPath))
            {
                Console.Write("Source BD-ROM path: ");
                bdromPath = Console.ReadLine();
            }

            // Prompt user for path to output MKV file
            while (string.IsNullOrWhiteSpace(mkvPath))
            {
                Console.Write("Output MKV path: ");
                mkvPath = Console.ReadLine();
            }

            Controller.ScanStarted += ControllerOnScanStarted;
            Controller.ScanSucceeded += ControllerOnScanSucceeded;

            Controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;

            Controller.SetEventScheduler();

            if (Scan(bdromPath, mkvPath))
            {
                if (Convert())
                {
                    Logger.Info("Muxing succeeded!");
                }
                else
                {
                    Logger.Error("Muxing failed!");
                }
            }
            else
            {
                Logger.Error("Scanning failed!");
            }

            Console.WriteLine();
            Console.WriteLine("*** BDhero CLI Finished - press <ENTER> to exit ***");
            Console.WriteLine();
            Console.Read();
        }

        private static void LogDirectoryPaths()
        {
            Logger.InfoFormat("IsPortable = {0}", DirectoryLocator.Instance.IsPortable);
            Logger.InfoFormat("InstallDir = {0}", DirectoryLocator.Instance.InstallDir);
            Logger.InfoFormat("ConfigDir = {0}", DirectoryLocator.Instance.ConfigDir);
            Logger.InfoFormat("PluginDir = {0}", DirectoryLocator.Instance.PluginDir);
            Logger.InfoFormat("LogDir = {0}", DirectoryLocator.Instance.LogDir);
        }

        private static bool Scan(string bdromPath, string mkvPath)
        {
            var scanTask = Controller.CreateScanTask(bdromPath, mkvPath);
            scanTask.Start();
            return scanTask.Result;
        }

        private static bool Convert()
        {
            var convertTask = Controller.CreateConvertTask();
            convertTask.Start();
            return convertTask.Result;
        }

        private static void ShowHelp(int exitCode = 0)
        {
            var exeName = ExeFileName;
            var usage = new Usage(exeName).TransformText();
            Console.Error.WriteLine(usage);
            Environment.Exit(exitCode);
        }

        private static void ShowVersion()
        {
            Console.Error.WriteLine("{0} v{1} - compiled {2}", AssemblyUtils.GetAssemblyName(), AssemblyUtils.GetAssemblyVersion(), AssemblyUtils.GetLinkerTimestamp());
        }

        private static void ControllerOnScanStarted(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
        }

        private static void ControllerOnScanSucceeded(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private static void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, (progressProvider.PercentComplete / 100.0).ToString("P"),
                                     progressProvider.Status,
                                     progressProvider.RunTime, progressProvider.TimeRemaining);
            if (progressProvider.State == ProgressProviderState.Running)
            {
                Console.WriteLine("\r{0}", line);
            }
            else
            {
                Console.WriteLine(line);
            }
        }
    }
}
