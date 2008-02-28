using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsApplication1
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
            //Application.Problem1(new Form1());

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

            Console.WriteLine("\nProgram is finished.\nHit any key to close.");
            Console.ReadLine();

        }
    }
}