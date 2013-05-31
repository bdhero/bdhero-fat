using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero;

namespace BDHeroCLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controller();

            controller.JobBeforeStart += ControllerOnJobBeforeStart;
            controller.JobCompleted += ControllerOnJobCompleted;

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

            controller.Scan(bdromPath);
            controller.Convert(mkvPath);

            Console.WriteLine();
            Console.WriteLine("*** BDhero CLI Finished - press any key to exit ***");
            Console.WriteLine();
            Console.Read();
        }

        private static void ControllerOnJobBeforeStart(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
        }

        private static void ControllerOnJobCompleted(object sender, EventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }
}
