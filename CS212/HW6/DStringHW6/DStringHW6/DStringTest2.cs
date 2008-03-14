using System;
using System.Collections.Generic;
using System.Text;
using DynamicString;
using System.IO;

namespace DStringHW6
{
    class DStringTest2
    {
        DString ds;
        Dictionary<DString, DString> mapping;
        DString dsMapped;

        public void run()
        {
            ds = Problem1();
            mapping = Problem2();
            dsMapped = Problem3();
            Problem4();
        }

        public DString Problem1()
        {

            bool fileFound = false;
            DString ds = new DString();

            while (fileFound == false)
            {
                Console.Write("Please enter a file path: ");
                String filepath = Console.ReadLine();

                try
                {
                    FileStream fs = File.Open(filepath, System.IO.FileMode.Open);
                    ds = new DString(fs);
                    fileFound = true;
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found.\n");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Please enter file path in correct format.\n");
                }

            }

            Console.WriteLine(ds.FirstWord());
            return ds;
            
        }//end Problem1

        public Dictionary<DString,DString> Problem2()
        {
            String pairs = Console.ReadLine();
            Console.WriteLine(pairs);
            String[] pairsArray = pairs.Split(new char[] { '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<DString,DString> pairsDict = new Dictionary<DString,DString>();

            //fill map with user input
            for (int i = 0; i < pairsArray.Length; i+=2)
            {
                //in case of multiple values for a key, it gets the first value
                if(!pairsDict.ContainsKey(pairsArray[i]) && (i+1 < pairsArray.Length) )
                {
                    pairsDict.Add(pairsArray[i], pairsArray[i + 1]);
                }
            }

            //map keys for the rest of the alphabet(upper and lower)
            String key = "";
            for (int i = 65; i <= 122; i++)
            {
                key = char.ConvertFromUtf32(i);
                if (!pairsDict.ContainsKey(key))
                {
                    pairsDict.Add(key, key);
                }
            }

            //print mapping
            foreach (KeyValuePair<DString, DString> k in pairsDict)
            {
                Console.WriteLine(k.ToString());
            }

            return pairsDict;

        }//end Problem2

        public DString Problem3()
        {
            DString d = new DString();
            char[] dsCharArray = ds.ToCharArray();

            String key = "";
            for (int i = 0; i < dsCharArray.Length; i++)
            {
                key = new DString(dsCharArray[i]);
                if (mapping.ContainsKey(key))
                {
                    dsCharArray[i] = mapping[key].FirstChar();
                }
                Console.WriteLine(dsCharArray[i]);
                d.Append(dsCharArray[i]);
            }

            Console.WriteLine(d);
            return d;
        }

        public DStringCollection Problem4()
        {
            DStringCollection arrayList = ds.Split("\n");
            DString alphaNumer = DString.Alphas();
            alphaNumer.Append(DString.Numbers());

            Console.WriteLine(alphaNumer);
            
            foreach (DString d in arrayList)
            {
                d.KeepAll(alphaNumer);
                Console.WriteLine(d);
            }

            return arrayList;
        }//end of Problem4
    }
}
