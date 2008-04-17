/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * Main program
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SetProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Console.WriteLine("Hello World");

            SetTest test = new SetTest();
            Console.WriteLine(test.tSet());
            Console.WriteLine(test.tSetCapacity());
            Console.WriteLine(test.tSetIEquality());
            Console.WriteLine(test.tClone());
            Console.WriteLine(test.tValues());
            Console.WriteLine(test.tList());
            Console.WriteLine(test.tAdd());
            Console.WriteLine(test.tAddRangeICol());
            Console.WriteLine(test.tAddRangeIDict());
            Console.WriteLine(test.tContains());
            Console.WriteLine(test.tIEnum());


            Console.ReadLine();
        }
    }
}