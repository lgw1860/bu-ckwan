using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DStringHW6
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
            Application.Run(new DStringHW6());
            
            //run from console
            /*
            DStringTest2 dst2 = new DStringTest2();
            dst2.run();
            Console.ReadLine();
             */
        }
    }
}