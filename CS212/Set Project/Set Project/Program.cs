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
            Application.Run(new Form1());

            Console.WriteLine("Hello World");

            Set<string> s = new Set<string>();
            //string test = (string)s.Clone();
            //Console.WriteLine(test);

            //Console.WriteLine(s.Add(null));
            s.Add("dog");
            s.Add("cat");
            s.Add("bird");

            Console.WriteLine("cat: " + s.Contains("cat"));
            Console.WriteLine("dinosaur: " + s.Contains("dinosaur"));

            /*
            foreach (string k in s)
            {
                Console.WriteLine(k.ToString());
            }
             */

            foreach (string i in s.List)
            {
                Console.Write(i + "->");

            }
            



            Console.ReadLine();
        }
    }
}