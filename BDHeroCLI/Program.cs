using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BDHero;

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
            Controller.JobBeforeStart += ControllerOnJobBeforeStart;
            Controller.JobSucceeded += ControllerOnJobSucceeded;
            Controller.LoadPlugins();

            string bdromPath = "",
                   mkvPath = "";

            if (args.Length == 2)
            {
                bdromPath = args[0];
                mkvPath = args[1];
            }
            else if (args.Length != 0)
            {
                Console.Error.WriteLine("Usage: {0} BDROM_PATH OUTPUT_MKV_PATH");
                Environment.Exit(1);
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

            if (Controller.Scan(bdromPath))
            {
                if (Controller.Convert(mkvPath))
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

        private static void ControllerOnJobBeforeStart(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
        }

        private static void ControllerOnJobSucceeded(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }
}
