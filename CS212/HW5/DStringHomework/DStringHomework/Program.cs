using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DStringApp
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
            Application.Run(new DStringHWButtons());

            Console.WriteLine("Christopher Kwan  U37-02-3645  ckwan@bu.edu\n");
            Console.WriteLine("CS212 Paradigms Lab 05 2/28/08\n");
            Console.WriteLine("DString Homework\n");

            DStringHomework HW = new DStringHomework();
            /*
            HW.Problem1();
            HW.Problem2();
            HW.Problem3();
            HW.Problem4();
            HW.Problem5();
            HW.Problem6();
            HW.Problem7();
            HW.Problem8();
            HW.Problem9();
            HW.Problem10();
             */

            Console.WriteLine("\nProgram is finished.");
            Console.WriteLine("\nHit any key to close.");
            //Console.ReadLine();

        }
    }
}