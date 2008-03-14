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
        String filepath;                       //1
        DString ds;                            //1
        Dictionary<DString, DString> mapping;  //2
        DString dsMapped;                      //3
        DStringCollection dsCollect;           //4
        ArrayList dsCollectList;               //5
        ArrayList origFileList;                //7
        String largeString;                    //8
        String asciiString;                    //9
        String backCap;                        //10

        public void run()
        {
            Console.WriteLine("\nChristopher Kwan\tckwan@bu.edu\tU37-02-3645");
            Console.WriteLine("\nCS212 Paradigms Lab 06 DString Homework");

            ds = Problem1();
            mapping = Problem2();
            dsMapped = Problem3();
            dsCollect = Problem4(dsMapped);
            dsCollectList = Problem5(ds);//(dsCollect);
            Problem6(dsCollectList);
            origFileList = Problem7(filepath);
            largeString = Problem8(origFileList);
            asciiString = Problem9(largeString);
            backCap = Problem10(largeString);

            Console.WriteLine("-==End of Program==-");
        }

        public DString Problem1()
        {
            Console.WriteLine("\n==Problem 1==");

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
                catch (NotSupportedException)
                {
                    Console.WriteLine("Path format is not supported.\n");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Unauthorized access");
                }
                catch
                {
                    Console.WriteLine("Fail. Please try again.");
                }

            }

            Console.WriteLine(ds);
            Console.WriteLine("\n==End 1==");
            return ds;
            
        }//end Problem1

        public Dictionary<DString,DString> Problem2()
        {
            Console.WriteLine("\n==Problem 2==");

            Console.Write("Please enter case-sensitive pairs of this kind: a=b c=d e=z: ");
            String pairs = Console.ReadLine();
            
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
            Console.WriteLine("Mapping: ");
            foreach (KeyValuePair<DString, DString> k in pairsDict)
            {
                Console.WriteLine(k.ToString());
            }

            Console.WriteLine("\n==End 2==");
            return pairsDict;

        }//end Problem2

        public DString Problem3()
        {
            Console.WriteLine("\n==Problem 3==");

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
                d.Append(dsCharArray[i]);
            }

            Console.WriteLine(d);
            Console.WriteLine("\n==End 3==");
            return d;
        }//end Problem3

        public DStringCollection Problem4(DString bigString)
        {
            Console.WriteLine("\n==Problem 4==");

            DStringCollection arrayList = bigString.Split("\n");

            DString alphaNumer = DString.Alphas();
            alphaNumer.Append(DString.Numbers());
            
            foreach (DString d in arrayList)
            {
                d.KeepAll(alphaNumer);
                Console.WriteLine(d);
            }

            Console.WriteLine("\n==End 4==");
            return arrayList;
        }//end Problem4

        public ArrayList Problem5(DString ds)//(DStringCollection dsCollect)
        {
            Console.WriteLine("\n==Problem 5==");
            
            ArrayList arrList = new ArrayList();
            DStringCollection dsList = ds.Split("\n");
            
            foreach (DString d in dsList)
            {
                d.ElimAll("\n\r");
                DStringCollection temp = d.Split(" ");
                arrList.Add(temp);
            }

            //print out DString collection
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
            Console.WriteLine("\n==End 5==");
            return arrList;
        }//end Problem5

        public void Problem6(ArrayList DSCollectList)
        {
            Console.WriteLine("\n==Problem 6==");

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

            Console.WriteLine("\nPlease see \"aaa.txt\" in the \"Output\" folder"); 
            Console.WriteLine("\n==End 6==");
        }//end Problem6

        public ArrayList Problem7(String filepath)
        {
            Console.WriteLine("\n==Problem 7==");
            ArrayList dsArrayList = new ArrayList();
            StreamReader reader = new StreamReader(filepath);
            while (!reader.EndOfStream)
            {
                String line = reader.ReadLine();
                dsArrayList.Add(line);
            }
            reader.Close();

            //print out arraylist
            for (int i = 0; i < dsArrayList.Count; i++)
            {
                Console.WriteLine(i + ": " + (DString)dsArrayList[i].ToString());
            }
            Console.WriteLine("\n==End 7==");
            return dsArrayList;
        }//end Problem7

        public String Problem8(ArrayList dsArrayList)
        {
            Console.WriteLine("\n==Problem 8==");
            DString d = new DString(dsArrayList);
            String largeString = d.ToString();
            Console.WriteLine(largeString);
            Console.WriteLine("\n==End 8==");
            return largeString;
        }//end Problem8

        public String Problem9(String largeString)
        {
            Console.WriteLine("\n==Problem 9==");

            DString d = new DString(largeString);
            d.ElimAll("\n");
            
            char[] dCharArray = d.ToCharArray();
            int[] valuesArray = new int[dCharArray.Length];
            DString valueString = new DString();
            
            for (int i = 0; i < dCharArray.Length; i++)
            {
                valuesArray[i] = char.ConvertToUtf32(dCharArray[i].ToString(), 0);
                valueString.Add(valuesArray[i]);
                valueString.Add(" ");
            }

            Console.WriteLine(valueString);
            Console.WriteLine("\n==End 9==");
            return valueString.ToString();
        }//end Problem9

        public String Problem10(String largeString)
        {
            Console.WriteLine("\n==Problem 10==");
            
            DString backCap = new DString(largeString);
            backCap = backCap.Reverse();
            backCap = backCap.ToUpper();

            DString consonentsPlus = DString.Alphas();
            consonentsPlus.ChopRight(consonentsPlus.Length / 2); //remove lowercase
            consonentsPlus.ElimAll("AEIOU");//remove vowels
            consonentsPlus.Add("\n ");//add newline and space

            backCap.KeepAll(consonentsPlus.ToString()); //keep only consonents,newline,space
            backCap.TrimMiddle(' ');  //remove excess spaces
            
            Console.WriteLine(backCap);
            Console.WriteLine("\n==End 10==");
            return backCap.ToString();
        }//end Problem10
    }
}
