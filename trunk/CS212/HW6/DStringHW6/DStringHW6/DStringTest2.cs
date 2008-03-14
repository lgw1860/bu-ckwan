using System;
using System.Collections.Generic;
using System.Text;
using DynamicString;
using System.IO;
using System.Collections;

namespace DStringHW6
{
    class DStringTest2
    {
        String filepath;
        DString ds;
        Dictionary<DString, DString> mapping;
        DString dsMapped;
        DStringCollection dsCollect;
        ArrayList dsCollectList;
        ArrayList origFileList;

        public void run()
        {
            ds = Problem1();
            mapping = Problem2();
            dsMapped = Problem3();
            dsCollect = Problem4(dsMapped);
            dsCollectList = Problem5(dsCollect);
            Problem6(dsCollectList);
            origFileList = Problem7(filepath);
        }

        public DString Problem1()
        {

            bool fileFound = false;
            DString ds = new DString();

            while (fileFound == false)
            {
                Console.Write("Please enter a file path: ");
                filepath = Console.ReadLine();

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
        }//end Problem3

        public DStringCollection Problem4(DString bigString)
        {
            DStringCollection arrayList = bigString.Split("\n");
            DString alphaNumer = DString.Alphas();
            alphaNumer.Append(DString.Numbers());

            Console.WriteLine(alphaNumer);
            
            foreach (DString d in arrayList)
            {
                d.KeepAll(alphaNumer);
                Console.WriteLine(d);
            }

            return arrayList;
        }//end Problem4

        public ArrayList Problem5(DStringCollection dsCollect)
        {
            ArrayList arrList = new ArrayList();
            //DStringCollection dsList = bigString.Split("\n");
            DStringCollection dsList = dsCollect;
            foreach (DString d in dsList)
            {
                d.ElimAll("\n\r");
                DStringCollection temp = d.Split();
                arrList.Add(temp);
            }

            for (int i = 0; i < arrList.Count; i++)
            {
                Console.Write(i + ": ");
                foreach (DString d in (DStringCollection)arrList[i])
                {
                    d.ElimAll("\n\r");
                    Console.Write(d);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            return arrList;
        }//end Problem5

        public void Problem6(ArrayList DSCollectList)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            dir = dir.Parent.Parent;    //get out of bin/debug
            dir = dir.CreateSubdirectory("Output");
            StreamWriter writer = new StreamWriter(dir.ToString()+"\\aaa.txt");

            foreach (DStringCollection collect in DSCollectList)
            {
                foreach (DString d in collect)
                {
                    writer.WriteLine(d.ToString());
                }
            }
            writer.Flush();
            writer.Close();
        }//end Problem6

        public ArrayList Problem7(String filepath)
        {
            ArrayList dsArrayList = new ArrayList();
            StreamReader reader = new StreamReader(filepath);
            while (!reader.EndOfStream)
            {
                String line = reader.ReadLine();
                dsArrayList.Add(line);
            }
            reader.Close();

            for (int i = 0; i < dsArrayList.Count; i++)
            {
                Console.WriteLine(i + ": " + (DString)dsArrayList[i].ToString());
            }

            return dsArrayList;
        }
    }
}
