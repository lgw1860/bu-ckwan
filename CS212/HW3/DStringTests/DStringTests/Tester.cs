/*
 * Christopher Kwan     ckwan@bu.edu    U37-02-3645
 * CS212 C# Devlin
 * Assignment #3 - Testing DString function
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicString
{
    class Tester
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Christopher Kwan\tckwan@bu.edu\tU37-02-3645");
            Console.WriteLine("\nTests of DString functions:");
            Console.WriteLine("---------------------------");
            
            Tester t = new Tester();

            t.testAdd();
            t.testDecimalize();
            t.testToBoolean();
            t.testPadRight();
            t.testForceRightSpace();
            t.testRemove();
            t.testDStringIntChar();
            t.testElimAll();

            Console.WriteLine("\n-End of Program-");
            Console.ReadLine();
        }

        /**
         * Test of Add(object).
         * 
         * Limits:
         *  - trying to add a null causes an exception.
         * 
         * Fix:
         * public int Add( Object value )  
		 *   {
         *      if(value != null)
         *      {
		 *	        int pos = Value.Length+1;
		 *	        Value += value.ToString();
		 *	        return pos;
         *      }
		 *  }
         */
        public void testAdd()
        {
            Console.WriteLine("\n\nTesting Add(object): \n");
            
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

        /**
         * Test of Decimalize(int).
         * 
         * Limits: passed all tests.
         */
        public void testDecimalize()
        {

            Console.WriteLine("\n\nTesting Decimalize(int): \n");

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

        /**
         * Test of PadRight(int, char).
         * 
         * Limits:
         *  - negative padding amount causes an exception.
         * 
         * Fix:
         *  public DString PadRight(int totalWidth,char paddingChar)
		 *  {
		 *      if(totalWidth < 0)
         *      {
         *          Value = Value.PadRight(0,paddingChar);
         *      }else
         *      {
         *          Value = Value.PadRight(totalWidth,paddingChar);
         *      }
         *      return this;
		 *  }
         * 
         */
        public void testPadRight()
        {
            Console.WriteLine("\n\nTesting PadRight(int, char): \n");

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

        /**
         * Test of constructor DString(int, char).
         * 
         * Limits:
         *  - putting in a very large int (but not quite a long) causes an exception.
         * 
         * Fix:
         * //5000 is arbitrary yet managable choice for upper string size limit
         * public DString(int count,char ch)
		 * {
         *      int upperLimit = 5000;
		 *      if ( count > 0 && <= upperLimit)
         *      {
		 *		    _strBuffer = new StringBuilder(new string(ch,count));
		 *      }else if (count > upperLimit)
         *      {
         *          _strBuffer = new StringBuilder(new string(ch,upperLimit));
         *      }else
         *      {
		 *		    _strBuffer = new StringBuilder(@"");
		 *      }
         * }
         * 
         */
        public void testDStringIntChar()
        {
            Console.WriteLine("\n\nTesting DString(int, char): \n");
            
            //very large int (1000000000, not quite a long)
            try
            {
                DString ds1 = new DString(1000000000, 'A');
                Console.WriteLine("PASS - very large int (1000000000, not quite a long): " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - very large int (1000000000, not quite a long)");
            }

            //negative int
            try
            {
                DString ds1 = new DString(-50, 'A');
                Console.WriteLine("PASS - negative int: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative int");
            }

            //space character
            try
            {
                DString ds1 = new DString(5, ' ');
                Console.WriteLine("PASS - space character: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - space character");
            }



        }

        /**
         * Test of ToBoolean(DString).
         * 
         * Limits: passed all tests.
         */
        public void testToBoolean()
        {
            Console.WriteLine("\n\nTesting ToBoolean(DString): \n");

            //string
            try
            {
                DString ds1 = new DString("cat");
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - string: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - string");
            }

            //char
            try
            {
                DString ds1 = new DString('A');
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - char: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - char");
            }

            //int
            try
            {
                DString ds1 = new DString(39423432);
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - int: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - int");
            }


            //double
            try
            {
                DString ds1 = new DString(39423432.4543535435335);
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - double: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - double");
            }

            //false bool
            try
            {
                DString ds1 = new DString(false);
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - false bool: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - false bool");
            }

            //true bool
            try
            {
                DString ds1 = new DString(true);
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - true bool: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - true bool");
            }

            //empty DString
            try
            {
                DString ds1 = new DString();
                DString.ToBoolean(ds1);
                Console.WriteLine("PASS - empty DString: " + ds1.ToString() + " " + DString.ToBoolean(ds1));
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - empty DString");
            }

        }

        /**
         * Test of Remove(int, int).
         * 
         * Limits: passed all tests.
         */
        public void testRemove()
        {

            Console.WriteLine("\n\nTesting Remove(int, int): \n");

            //negative index
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(-50, 1);
                Console.WriteLine("PASS - negative index: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative index");
            }

            //negative length
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(1, -50);
                Console.WriteLine("PASS - negative length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative length");
            }

            //negative index and length
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(-50, -50);
                Console.WriteLine("PASS - negative index and length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - negative index and length");
            }

            //index exceeding actual length
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(50, 1);
                Console.WriteLine("PASS - index exceeding actual length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - index exceeding actual length");
            }

            //length exceeding actual length
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(1, 50);
                Console.WriteLine("PASS - length exceeding actual length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - length exceeding actual length");
            }

            //index and length exceeding actual length
            try
            {
                DString ds1 = new DString("rhinoceros");
                ds1.Remove(50, 50);
                Console.WriteLine("PASS - index and length exceeding actual length: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - index and length exceeding actual length");
            }

        }

        /**
         * Test of ForceRightSpace().
         * 
         * Limits: passed all tests.
         */
        public void testForceRightSpace()
        {
            Console.WriteLine("\n\nTesting ForceRightSpace(): \n");

            //empty string
            try 
            {
                DString ds1 = new DString("");
                ds1.ForceRightSpace();
                Console.WriteLine("PASS - empty string: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - empty string");
            }

            //leading space
            try
            {
                DString ds1 = new DString(" cat");
                ds1.ForceRightSpace();
                Console.WriteLine("PASS - leading space: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - leading space");
            }

            //newline
            try
            {
                DString ds1 = new DString("\n");
                ds1.ForceRightSpace();
                Console.WriteLine("PASS - newline: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - newline");
            }

            //tab
            try
            {
                DString ds1 = new DString("\t");
                ds1.ForceRightSpace();
                Console.WriteLine("PASS - tab: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - tab");
            }

        }

        /**
         * Test of ElimAll(string).
         * 
         * Limits: passed all tests.
         */
        public void testElimAll()
        {

            Console.WriteLine("\n\nTesting ElimAll(string): \n");

            //empty control string
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("");
                Console.WriteLine("PASS - empty control string: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - empty control string");
            }

            //space control string
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll(" ");
                Console.WriteLine("PASS - space control string: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - space control string");
            }

            //control in DString but with ending spaces
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("cac  ");
                Console.WriteLine("PASS - control in DString but with ending space: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - control in DString but with ending space");
            }


            //control in DString but with leading spaces
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("  I");
                Console.WriteLine("PASS - control in DString but with leading spaces: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - control in DString but with leading spaces");
            }

            //control not in DString
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("xyz");
                Console.WriteLine("PASS - control not in DString: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - control not in DString");
            }

            //control not in DString except one letter - "xyaz"
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("xyaz");
                Console.WriteLine("PASS - control not in DString except one letter - xyaz: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - control not in DString except one letter - xyaz");
            }

            //control longer than in DString - Youareafernnotacactusdogafern
            try
            {
                DString ds1 = new DString("I am a cactus");
                ds1.ElimAll("Youareafernnotacactusdogafern");
                Console.WriteLine("PASS - control longer than in DString - Youareafernnotacactusdogafern: " + ds1.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("FAIL - control longer than in DString - Youareafernnotacactusdogafern");
            }

        }
    }
}
