using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicString
{
    class Tester
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hey Justin :(");

            MyString ms = new MyString();

            Console.WriteLine(ms.Name);
            ms.Name = "Ermac";
            Console.WriteLine(ms.Name);

            //DynamicString.DString ds = new DynamicString.DString();
            DString ds = new DString();
            ds.Add("heyguysimads");
            Console.WriteLine( ds.ToString() );

            Console.WriteLine(ds.Add(1));
            Console.WriteLine(ds.ToString());

            Tester t = new Tester();
            Console.WriteLine(t.testAdd());

            Console.ReadLine();
        }

        public int testAdd()
        {
            //DynamicString.DString ds1 = new DynamicString.DString("goDaddy");
            DString ds1 = new DString("goDaddy");
            try
            {
                //return( ds1.Add(8) );
                //ds1.Add(8);
                //Console.WriteLine("Adding an integer to a DStringFixed: PASS");

                ds1.Add(null);
                Console.WriteLine("Adding a null to a DString: PASS");
            }
            catch (Exception e)
            {
                //Console.WriteLine("Cannot add null to DString");
                //Console.WriteLine("Adding an integer to a DString: FAIL");
                Console.WriteLine("Adding a null to a DString: FAIL");
            }
            return 888;

        }
    }
}
