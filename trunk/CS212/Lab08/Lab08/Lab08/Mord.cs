using System;
using System.Collections.Generic;
using System.Text;

namespace Lab08
{
    class Mord
    {
        public enum eDirection
        {
            LittleGood, BigGood
        };

        double[] values = { 3.0, 5.0, 6.0, 7.0, 1.0};
            //{ 3.0, 5.0, 6.0, 7.0, 1.0, 5.0 };
            //{ 22, 33, 44, 55, 22, 55, 55, 66 };

        sbyte[] tieBreakValues = { 2, 3, 1, 4, 5};//{ 2, 3, 1, 4, 5, 6 };
        //= { 20, 20, 20, 30 };
            //{ 22, 33, 44, 55, 22, 55, 55, 66 };
            
            //{ 345.0, 477.0, 211.0 };
        LinkedList<int> list = new LinkedList<int>();

        public void run()
        {
            /*
            values = new double[100];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = i * .50;
            }
            Array.Reverse(values);
            */

            GetPositionMap();

            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine("{0}: {1}", i, values[i]);
            }

            op test = new op(GetMord);
            sbyte[] results = test(eDirection.BigGood);

            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine("{0}: {1}: {2}", i, values[i], results[i]);
            }

            GetKord(tieBreakValues);
        }

        public delegate sbyte[] op(eDirection dir);


        protected Dictionary<double, sbyte> GetMap(eDirection dir)
        {
            Dictionary<double, sbyte> map = new Dictionary<double, sbyte>();
            double[] sortedValues = (double[])values.Clone();
            Array.Sort(sortedValues); //sort a copy of the values array

            //Reverse if BigGood
            if (dir == eDirection.BigGood)
            {
                Array.Reverse(sortedValues);
            }

            sbyte rank = 1;
            sbyte skippedRanks = 0;

            //rank the sorted values array and create a mapping
            for (int i = 0; i < sortedValues.Length; i++)
            {
                if (i == 0)
                {
                    //mordValues[i] = rank;
                    if (!map.ContainsKey(sortedValues[i]))
                    {
                        map.Add(sortedValues[i], rank);
                    }
                }
                else
                {
                    if (sortedValues[i] == sortedValues[i - 1])
                    {
                        skippedRanks++;
                        //mordValues[i] = rank;
                        if (!map.ContainsKey(sortedValues[i]))
                        {
                            map.Add(sortedValues[i], rank);
                        }
                    }
                    else
                    {
                        rank++;
                        rank += skippedRanks;
                        skippedRanks = 0;
                        //rank++;
                        //mordValues[i] = rank;
                        if (!map.ContainsKey(sortedValues[i]))
                        {
                            map.Add(sortedValues[i], rank);
                        }
                    }
                }
            }//end for
            return map;
        }

        protected sbyte[] GetMord(eDirection dir)
        {
            sbyte[] mordValues = new sbyte[values.Length];
           
            Dictionary<double, sbyte> map = GetMap(dir);

            //use mapping to place ranks in original order
            for (int j = 0; j < values.Length; j++)
            {
                if (map.ContainsKey(values[j]))
                {
                    mordValues[j] = map[values[j]];
                }
            }

            foreach (KeyValuePair<double, sbyte> k in map)
            {
                Console.WriteLine(k.ToString());
            }

            return mordValues;
        }//end GetMord

        protected Dictionary<double, sbyte> GetPositionMap()
        {
            Dictionary<double, sbyte> map = new Dictionary<double, sbyte>();
            for (int i = 0; i < values.Length; i++)
            {
                map.Add(values[i], (sbyte)i);
            }

            foreach (KeyValuePair<double, sbyte> k in map)
            {
                Console.WriteLine(k.ToString());
            }

            return map;
        }

        protected void GetKord(sbyte[] tiebrk)
        {
            sbyte[] origMord = GetMord(eDirection.BigGood);
            sbyte[] revisedMord = (sbyte[])origMord.Clone();
            Dictionary<sbyte, sbyte> tieBrkMap = new Dictionary<sbyte, sbyte>();
            //tieBrkMap.Values.

            double[] sortedValues = (double[])values.Clone();
            Array.Sort(sortedValues);

            ///BIGGOOD
            Array.Reverse(sortedValues);

            sbyte[] kordValues = new sbyte[values.Length];
            //Dictionary<double, sbyte> map = GetMap(eDirection.BigGood);
            Dictionary<double, sbyte> posMap = GetPositionMap();
            for (int j = 0; j < kordValues.Length; j++)
            {
                //if (posMap.ContainsKey(j))
                //{
                    kordValues[j] = posMap[sortedValues[j]];
                //}

            }

            Console.WriteLine("\nIf the original values are");
            for (int i = 0; i < values.Length; i++)
            {
                Console.Write("{0} ", values[i]);
            }

            Console.WriteLine("\nwith BigGood, the mord values will be");
            for (int i = 0; i < origMord.Length; i++)
            {
                Console.Write("{0} ", origMord[i]);
            }

            Console.WriteLine("\nso if the tiebrk values are");
            for (int i = 0; i < tiebrk.Length; i++)
            {
                Console.Write("{0} ", tiebrk[i]);
            }

            Console.WriteLine("\nand the kord values will be");
            for (int i = 0; i < kordValues.Length; i++)
            {
                Console.Write("{0} ", kordValues[i]);
            }
        }
    }
}
