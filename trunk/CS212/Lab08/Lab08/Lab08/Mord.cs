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

        double[] values;
        sbyte[] tieBreakValues;
        public delegate sbyte[] op(eDirection dir);

        public Mord(double[] values, sbyte[] tieBreakValues)
        {
            this.values = values;
            this.tieBreakValues = tieBreakValues;
        }

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

        public sbyte[] GetMord(eDirection dir)
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

            return mordValues;
        }//end GetMord

        protected Dictionary<double, sbyte> GetMapNoTies(eDirection dir)
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
            //sbyte skippedRanks = 0;

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
                        rank++;
                        //skippedRanks++;
                        //mordValues[i] = rank;
                        if (!map.ContainsKey(sortedValues[i]))
                        {
                            map.Add(sortedValues[i], rank);
                        }
                    }
                    else
                    {
                        rank++;
                        //rank += skippedRanks;
                        //skippedRanks = 0;
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

        protected sbyte[] GetMordNoTies(eDirection dir)
        {
            sbyte[] mordValues = new sbyte[values.Length];

            Dictionary<double, sbyte> map = GetMapNoTies(dir);

            //use mapping to place ranks in original order
            for (int j = 0; j < values.Length; j++)
            {
                if (map.ContainsKey(values[j]))
                {
                    mordValues[j] = map[values[j]];
                }
            }

            return mordValues;
        }//end GetMord

        protected Dictionary<double, sbyte> GetPositionMap()
        {
            Dictionary<double, sbyte> map = new Dictionary<double, sbyte>();
            for (int i = 0; i < values.Length; i++)
            {
                if(!map.ContainsKey(values[i]))
                    map.Add(values[i], (sbyte)i);
            }

            return map;
        }

        public sbyte[] GetKord(sbyte[] tiebrk, eDirection dir)
        {
            sbyte[] origMord = GetMord(eDirection.BigGood);
            sbyte[] revisedMord = GetMordNoTies(eDirection.BigGood);
           
            Dictionary<sbyte, sbyte> tieBrkMap = new Dictionary<sbyte, sbyte>();

            double[] sortedValues = (double[])values.Clone();
            Array.Sort(sortedValues);

            //for BIGGOOD
            if (dir == eDirection.BigGood)
            {
                Array.Reverse(sortedValues);
            }

            sbyte[] kordValues = new sbyte[values.Length];
            Dictionary<double, sbyte> posMap = GetPositionMap();
            for (int j = 0; j < kordValues.Length; j++)
            {
                if (posMap.ContainsKey(sortedValues[j]))
                {
                    kordValues[j] = posMap[sortedValues[j]];
                }

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

            Console.WriteLine("\nthe revised mord values will be");
            for (int i = 0; i < revisedMord.Length; i++)
            {
                Console.Write("{0} ", revisedMord[i]);
            }

            Console.WriteLine("\nand the kord values will be");
            for (int i = 0; i < kordValues.Length; i++)
            {
                Console.Write("{0} ", kordValues[i]);
            }

            return kordValues;
        }
    }
}
