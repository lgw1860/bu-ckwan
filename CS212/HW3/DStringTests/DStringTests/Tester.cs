using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicString
{
    class Tester
    {
        static void Main(string[] args)
        {
            Tester t = new Tester();
            //t.testAdd();
            //t.testCutout();
            t.testDecimalize();

            Console.ReadLine();
        }

        public void testAdd()
        {
            //DynamicString.DString ds1 = new DynamicString.DString("goDaddy");
            
            //adding null
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add(null);
                Console.WriteLine("PASS - Adding a null to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding a null to a DString");
            }

            //adding empty string
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add("");
                Console.WriteLine("PASS - Adding an empty string to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding an empty string to a DString");
            }

            //adding a space
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add(" ");
                Console.WriteLine("PASS - Adding a space to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding a space to a DString");
            }

            //adding a tab
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add("\t");
                Console.WriteLine("PASS - Adding a tab to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding a tab to a DString");
            }

            //adding another DString
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add(new DString("CROCODILE"));
                Console.WriteLine("PASS - Adding a DString to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding a DString to a DString");
            }

            //adding empty DString
            try
            {
                DString ds1 = new DString("kangaroo");
                ds1.Add(new DString());
                Console.WriteLine("PASS - Adding an empty DString to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding an empty DString to a DString");
            }

        }

        /*
         * Cutout(string, int, int)
         */
        public void testCutout()
        {
            //adding empty DString
            try
            {
                DString ds1 = new DString("kangaroo");
                string s = "kang";
                ds1.Cutout(s,-4,1);
                Console.WriteLine("PASS - Adding an empty DString to a DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Adding an empty DString to a DString");
            }

        }

        public void testDecimalize()
        {
            //negative int
            try
            {
                DString ds1 = new DString(-18);
                ds1.Decimalize(2);
                Console.WriteLine("PASS - Negative int: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - Negative int");
            }

            //negative number decimal places
            try
            {
                DString ds1 = new DString(18);
                ds1.Decimalize(-1);
                Console.WriteLine("PASS - negative number decimal places: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative number decimal places");
            }

            //large int number decimal places
            try
            {
                DString ds1 = new DString(18);
                ds1.Decimalize(100000000);
                Console.WriteLine("PASS - large int number of decimal places: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - large int number of decimal places");
            }

            //num of dec places is larger than those originally in DString
            try
            {
                DString ds1 = new DString(18.5);
                ds1.Decimalize(5);
                Console.WriteLine("PASS - num of dec places is larger than those originally in DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - num of dec places is larger than those originally in DString");
            }

            //num of dec places is smaller than those originally in DString
            try
            {
                DString ds1 = new DString(18.56789);
                ds1.Decimalize(2);
                Console.WriteLine("PASS - num of dec places is smaller than those originally in DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - num of dec places is smaller than those originally in DString");
            }
        }


    }
}
