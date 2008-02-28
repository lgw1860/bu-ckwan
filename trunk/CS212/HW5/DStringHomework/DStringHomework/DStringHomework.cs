using System;
using System.Collections.Generic;
using System.Text;
using DynamicString;
using System.IO;

namespace WindowsApplication1
{
    class DStringHomework
    {
        public void Problem1()
        {
            Console.WriteLine("\n---Problem 1---");

            Console.Write("Please enter the path of a file: ");

            try
            {
                //Read in a file into a single DString
                string filepath = Console.ReadLine();
                Console.WriteLine("Path: " + filepath);
                //string filepath = "C:\\code\\svnS08\\CS212\\HW5\\test.txt";
                DString ds = new DString(filepath, System.IO.FileMode.Open);

                //Convert single DString into array or DStrings split by newlines
                DStringCollection dsCollect = ds.Split("\n", true);
                DString[] dsArr = dsCollect.ToArray();

                //Remove all blank lines and create a new single DString separated by '$'
                DString dsMoney = new DString("");
                for (int i = 0; i < dsArr.Length; i++)
                {
                    //remove carriage returns and newlines
                    //carriage returns tend to mess up string concat
                    dsArr[i].ElimAll("\r");
                    dsArr[i].ElimAll("\n");

                    if (!dsArr[i].IsBlank())
                    {
                        dsMoney.Append(dsArr[i].ToString());
                        dsMoney.Append("$");
                    }
                }

                //Remove all newlines and carriage returns
                dsMoney.ElimAll("\n");
                dsMoney.ElimAll("\r");

                //Remove all vowels
                DString vowels = new DString("aeiouAEIOU");
                dsMoney.ElimAll(vowels.ToString());

                //Remove all numbers
                dsMoney.ElimAll(DString.Numbers().ToString());

                //Remove all letters (A-M)
                DString AthruM = DString.Alphas();  //start with whole alphabet and remove stuff
                AthruM = (AthruM.Substring(0, 13) + AthruM.Substring(26 + 0, 13));
                dsMoney.ElimAll(AthruM);

                //Create array of DStrings separated by '$'
                DStringCollection dsMoneyCollect = dsMoney.Split("$");
                DString[] dsMoneyArray = dsMoneyCollect.ToArray();

                //Print out array, line by line, with '$' at end of each line
                foreach (DString i in dsMoneyArray)
                {
                    i.Append("$");
                    Console.WriteLine(i);
                }

            }
            catch (Exception e)
            {
                e = new FileNotFoundException();
                Console.WriteLine("\nFile not found.\n");
            }
            Console.WriteLine("---End of Problem 1---");

        }//end of Problem1

        public void Problem2()
        {
            Console.WriteLine("\n---Problem 2---");

            DString ds = new DString("Every good boy deserves fudge");
            
            //Separate ds into array of words
            DStringCollection dsCollect = ds.Split(" ");
            DString[] dsArr = dsCollect.ToArray();

            //Delete every other word starting with "Every"
            //(by not adding it to new DString)
            DString dsNew = new DString();
            //char firstChar = ' ';
            DString temp = new DString();
            for (int i = 0; i < dsArr.Length; i++)
            {
                if (i % 2 != 0)
                {
                    //Capitalize the string
                    //Assume capitalize means make all letters uppercase
                        //firstChar = dsArr[i].FirstChar();
                        //firstChar = char.ToUpper(firstChar);
                        //dsArr[i].ChopLeft();
                        //dsArr[i] = firstChar + dsArr[i];
                    temp = "";
                    foreach (char j in dsArr[i])
                    {
                        temp += char.ToUpper(j);
                    }
                    dsNew.Append(temp);
                    dsNew.Append(" ");  //space separator
                }
            }

            dsCollect = dsNew.Split(" ");
            dsArr = dsCollect.ToArray();

            //Print resulting array, one line per DString
            foreach(DString i in dsArr)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("---End of Problem 2---");

        }//end of Problem2

        public void Problem3()
        {
            Console.WriteLine("\n---Problem 3---");

            DString ds = new DString("OO Mau Mau, Papa OO Mau Mau");
            
            //Excise each occurance of "Mau"
            while (ds.Excise("Mau"))
            {
                ds.Excise("Mau");
            }

            //Capitalize the string
            //Assume capitalize means make all letters uppercase
            DString temp = new DString();
            foreach (char i in ds)
            {
                temp += char.ToUpper(i);
            }

            //Print it
            ds = temp;
            Console.WriteLine(ds);

            Console.WriteLine("---End of Problem 3---");

        }//end of Problem3

        public void Problem4()
        {
            Console.WriteLine("\n---Problem 4---");

            DString ds = new DString(
                "Happy Birthday, Mein Gott, Arriverderci Roma, The Sopranos, Michael Jordan, "
                + "Uncle Miltie, Elton John, John Hartford, Hartford Connecticut");

            //Map made from DString
            System.Collections.Specialized.StringDictionary map = ds.GetStringDictionary(" ");

            DString dsNew = new DString();
            DString tempKey = "";
            char firstLetter = ' ';
            foreach(string k in map.Keys)
            {
                //Capitalize first letter of each Key
                //b/c StringDictionary makes them all lowercase
                tempKey = new DString();    //reset tempKey
                tempKey.Append(k);
                firstLetter = char.ToUpper(tempKey.FirstChar());
                tempKey.ChopLeft();
                tempKey = firstLetter + tempKey;

                //Construct new DString of all key value pairs
                dsNew.Append(tempKey);
                dsNew.Append(":");
                dsNew.Append(map[k]);
                dsNew.Append(" \n");
            }

            //Remove all the commas
            dsNew.ElimAll(",");

            //Print the DString, with each pair on a separate line
            Console.WriteLine(dsNew);

            Console.WriteLine("---End of Problem 4---");

        }//end of Problem4



        public void Problem6()
        {
            Console.WriteLine("\n---Problem 6---");

            //Start with an empty DString and build up
            DString ds = new DString();
            ds += "There";
            ds += " ";
            ds += "are";
            ds += ' ';

            int x = 100;

            ds += x;
            ds += " ";
            ds += "ways to skin a cat";

            //Capitalize the whole thing
            DString temp = new DString();
            foreach (char i in ds)
            {
                temp += char.ToUpper(i);
            }

            ds = temp;
            //Print it out
            Console.WriteLine(ds);

            //Replace "WAYS" with "METHODS" and print resulting string out
            ds.ReplaceAll("WAYS", "METHODS");
            Console.WriteLine(ds);

            Console.WriteLine("---End of Problem 6---");

        }//end of Problem6

        public void Problem7()
        {

            Console.WriteLine("\n---Problem 7---");

            //Create array of about 50 doubles
            double[] doubleArr = new double[50];
            for (int i = 0; i < doubleArr.Length; i++)
            {
                doubleArr[i] = (double)i * 0.25;
            }

            //Construct a single DString of array with a space separator
            DString ds = new DString(doubleArr, " ");

            //Print it out
            Console.WriteLine(ds);

            Console.WriteLine("---End of Problem 7---");

        }//end of Problem7

        public void Problem8()
        {
            Console.WriteLine("\n---Problem 8---");

            //Map of integers(1->10) to floats(100->1000)
            Dictionary<int,float> map = new Dictionary<int,float>();

            for(int i=1; i<=10; i++)
            {
                map.Add(i, i * 100.0f);
            }

            //Bring map into a DString and print it out
            DString ds = new DString(map," ","\n");
            Console.WriteLine(ds);

            Console.WriteLine("---End of Problem 8---");

        }//end of Problem8

        public void Problem9()
        {
            Console.WriteLine("\n---Problem 9---");

            DString d = new DString("I am empty");
            Console.WriteLine(d);

            //Reverse DString by characters and print it out again
            d = d.Reverse();
            Console.WriteLine(d);

            Console.WriteLine("---End of Problem 9---");

        }//end of Problem9

        public void Problem10()
        {
            Console.WriteLine("\n---Problem 10---");

            //Take any given DString
            Console.Write("Please enter a DString: ");
            DString ds = Console.ReadLine();

            //Remove middle three characters
            int half = ds.Length/2;
            if (ds.Length <= 3) 
            {
                ds.Remove(0, ds.Length); //remove all
            }
            else if ((ds.Length > 3) && (ds.Length % 2 == 0))
            {
                ds.Remove(half - 2, 3); //remove forward middle three
            }
            else //length > 3 and odd
            {
                ds.Remove(half - 1, 3); //remove middle three
            }

            //Print result
            Console.WriteLine("Result: " + ds);

            Console.WriteLine("\n---End of Problem 10---");

        }//end of Problem10
    }
}
