﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero;

namespace BDHeroCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controller();
            controller.Scan(@"W:\BD\49123204_BLACK_HAWK_DOWN");
            Console.Read();
        }
    }
}