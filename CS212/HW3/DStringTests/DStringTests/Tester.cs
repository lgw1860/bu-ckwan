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
            //t.testDecimalize();
            //t.testPadRight();
            t.testDStringIntChar();

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

        public void testPadRight()
        {
            //padding amount greater than Dstring length
            try
            {
                DString ds1 = new DString("apple");
                ds1.PadRight(10, 'A');
                Console.WriteLine("PASS - padding amount greater than DString length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - padding amount greater than DString length");
            }

            //padding amount equal to Dstring length
            try
            {
                DString ds1 = new DString("apple");
                ds1.PadRight(5, 'A');
                Console.WriteLine("PASS - padding amount equal to Dstring length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - padding amount equal to Dstring length");
            }

            //padding amount less than Dstring length
            try
            {
                DString ds1 = new DString("apple");
                ds1.PadRight(3, 'A');
                Console.WriteLine("PASS - padding amount less than Dstring length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - padding amount less than Dstring length");
            }

            //negative padding amount
            try
            {
                DString ds1 = new DString("apple");
                ds1.PadRight(-1, 'A');
                Console.WriteLine("PASS - negative padding amount: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative padding amount");
            }

        }

        public void testDStringIntChar()
        {
            //very large int
            try
            {
                DString ds1 = new DString(1000000000, 'A');
                Console.WriteLine("PASS - very large int: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - very large int");
            }


        }
    }
}
