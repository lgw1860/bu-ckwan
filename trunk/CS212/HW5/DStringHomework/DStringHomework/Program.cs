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

            Console.WriteLine("DString Homework\n");

            DStringHomework HW = new DStringHomework();
            //HW.Problem1();
            //HW.Problem2();
            //HW.Problem3();
            //HW.Problem4();

            //HW.Problem6();
            //HW.Problem7();

            //HW.Problem9();
            //HW.Problem10();

            Console.ReadLine();

        }
    }
}