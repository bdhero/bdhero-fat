using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero;
using BDHero.Plugin;
using DotNetUtils;
using Mono.Options;
using ProcessUtils;

namespace BDHeroCLI
{
    static class Program
    {
        /// <summary>
        /// IMPORTANT: This must be the absolute FIRST line of code that runs to initialize logging!
        /// </summary>
        private static readonly Controller Controller = new Controller("bdhero-cli.log.config");

        /// <summary>
        /// Depends on <see cref="Controller"/> being initialized first.
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            string bdromPath = "",
                   mkvPath = "";
            bool? replace;

            var optionSet = new OptionSet
                {
                    { "h|?|help", v => ShowHelp() },
                    { "version", s => ShowVersion() },
                    { "v|verbose", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Debug },
                    { "w|warn", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Warn },
                    { "q|quiet", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Error },
                    { "s|silent", s => Logger.Logger.Repository.Threshold = log4net.Core.Level.Fatal },
                    { "i=|input=", s => bdromPath = s },
                    { "o=|output=", s => mkvPath = s },
                    { "y", s => replace = true },
                    { "n", s => replace = false }
                };

            var extraArgs = optionSet.Parse(args);

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

            Controller.JobBeforeStart += ControllerOnJobBeforeStart;
            Controller.JobSucceeded += ControllerOnJobSucceeded;
            Controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;
            Controller.LoadPlugins();

            if (Controller.Scan(bdromPath, mkvPath))
            {
                if (Controller.Convert())
                {
                    Console.WriteLine();
                    Console.WriteLine("MUXING SUCCEEDED!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("MUXING FAILED!");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("SCANNING FAILED!");
            }

            Console.WriteLine();
            Console.WriteLine("*** BDhero CLI Finished - press <ENTER> to exit ***");
            Console.WriteLine();
            Console.Read();
        }

        private static void ShowHelp(int exitCode = 0)
        {
            Console.Error.WriteLine("Usage: {0} -i BDROM_PATH -o OUTPUT_MKV_PATH", Path.GetFileName(AssemblyUtils.AssemblyOrDefault().Location));
            Environment.Exit(exitCode);
        }

        private static void ShowVersion(int exitCode = 0)
        {
            Console.Error.WriteLine("{0} v{1} - compiled {2}", AssemblyUtils.GetAssemblyName(), AssemblyUtils.GetAssemblyVersion(), AssemblyUtils.GetLinkerTimestamp());
            Environment.Exit(exitCode);
        }

        private static void ControllerOnJobBeforeStart(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
        }

        private static void ControllerOnJobSucceeded(object sender, EventArgs eventArgs)
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
