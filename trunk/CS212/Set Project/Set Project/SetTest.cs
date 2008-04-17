/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * SetTest
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SetProject
{
    public class SetTest
    {
        
        public object run()
        {
            //Tests for Set
            
            //Test for Set()
          
                Set<string> s1 = new Set<string>();
                //report


            return null;
            //return(s.Clone());

        }

      
        public string tSet()
        {
            string report = "\nSet<T>() Test:";
            try
            {
                Set<string> s = new Set<string>();
                report += "\n Set created successfully";
                report += "\n ToString: " + s.ToString();
            }
            catch (Exception)
            {
                report += "\n Set creation failed";
            }
            return report;
        }

        public string tSetCapacity()
        {
            int capacity = 3;
            string report = "\nSet<T>(capacity) Test:";
            try
            {
                Set<string> s = new Set<string>(capacity);
                report += "\n Set with initial capacity 3 created successfully";
                report += "\n ToString: " + s.ToString();

                report += "\n\n Adding < 3 elements to set: ";
                try
                {
                    s.Clear();
                    s.Add("A");
                    s.Add("B");
                    report += " Success\n Set: ";
                    foreach (string i in s)
                    {
                        report += i + " ";
                    }
                }
                catch (Exception)
                {
                    report += " Fail";
                }

                report += "\n\n Adding 3 elements to set: ";
                try
                {
                    s.Clear();
                    s.Add("A");
                    s.Add("B");
                    s.Add("C");
                    report += " Success\n Set: ";
                    foreach (string i in s)
                    {
                        report += i + " ";
                    }
                }
                catch (Exception)
                {
                    report += " Fail";
                }

                report += "\n\n Adding > 3 elements to set: ";
                try
                {
                    s.Clear();
                    s.Add("A");
                    s.Add("B");
                    s.Add("C");
                    s.Add("D");
                    report += " Success\n Set: ";
                    foreach (string i in s)
                    {
                        report += i + " ";
                    }
                }
                catch (Exception)
                {
                    report += " Fail";
                }

            }
            catch (Exception)
            {
                report += "\n Set Creation failed";
            }
            return report;
        }

        public string tSetIEquality()
        {
            string report = "\nSet<T>(InEquality) Test:";
            try
            {
                IEqualityComparer<string> ie = null;
                Set<string> s = new Set<string>(ie);
                report += "\n Set created successfully";
                report += "\n ToString: " + s.ToString();
            }
            catch (Exception)
            {
                report += "\n Set creation failed";
            }
            return report;
        }

        public string tClone()
        {
            string report = "\nClone() Test:";
            try
            {
                Set<string> s = new Set<string>();
                s.Add("A");
                s.Add("B");
                report += "\n Set: ";
                foreach (string i in s)
                    report += i + " ";

                Set<string> sClone = (Set<string>)s.Clone();
                report += "\n Clone of Set: ";
                foreach (string j in sClone)
                    report += j + " ";


                report += "\n Clone test passed successfully";
            }
            catch (Exception)
            {
                report += "\n Clone test failed";
            }
            return report;
        }

        public string tValues()
        {
            string report = "\nValues Test:";
            try
            {
                Set<string> s = new Set<string>();
                s.Add("A");
                s.Add("B");
                report += "\n Set: ";
                foreach (string i in s)
                    report += i + " ";

                report += "\n Values: ";
                foreach (string j in s.Values)
                    report += j + " ";


                report += "\n Values test passed successfully";
            }
            catch (Exception)
            {
                report += "\n Values test failed";
            }
            return report;
        }


        public string tList()
        {
            string report = "\nList Test:";
            try
            {
                Set<string> s = new Set<string>();
                s.Add("A");
                s.Add("B");
                report += "\n Set: ";
                foreach (string i in s)
                    report += i + " ";

                List<string> l = s.List;
                report += "\n List: ";
                foreach (string j in s.Values)
                    report += j + " ";
                report += "\n" + l.ToString();

                report += "\n List test passed successfully";
            }
            catch (Exception)
            {
                report += "\n List test failed";
            }
            return report;
        }

        public string tAdd()
        {
            Set<string> s = new Set<string>();
            string report = "\nAdd() Test:";

            report += "\n Adding elements to set: ";
            try
            {
                s.Clear();
                s.Add("A");
                s.Add("B");
                report += " Success\n Set: ";
                foreach (string i in s)
                {
                    report += i + " ";
                }
            }
            catch (Exception)
            {
                report += " Fail";
            }

            return report;
        }

        public string tAddRangeICol()
        {
            Set<string> s = new Set<string>();
            string report = "\nAddRange(ICollection) Test:";
            LinkedList<string> li = new LinkedList<string>();
            li.AddLast("cat");
            li.AddLast("dog");

            report += "\n ICollection<T>: ";
            foreach (string i in li)
                report += i + " ";

            report += "\n Adding elements from ICollection<T> to set: ";
            try
            {
                s.Clear();
                s.AddRange(li);
                report += " Success\n Set: ";
                foreach (string i in s)
                {
                    report += i + " ";
                }
            }
            catch (Exception)
            {
                report += " Fail";
            }

            return report;
        }

        public string tAddRangeIDict()
        {
            Set<string> s = new Set<string>();
            string report = "\nAddRange(IDictionary) Test:";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("mouse", null);
            dict.Add("cat", null);

            report += "\n IDictionary<T>: ";
            foreach (string i in dict.Keys)
                report += i + " ";

            report += "\n Adding elements from IDictionary<T> to set: ";
            try
            {
                s.Clear();
                s.AddRange(dict);
                report += " Success\n Set: ";
                foreach (string i in s)
                {
                    report += i + " ";
                }
            }
            catch (Exception)
            {
                report += " Fail";
            }

            return report;
        }

        public string tContains()
        {
            string report = "\nContains() Test:";
            report += "\n Set: ";
            Set<string> s = new Set<string>();
            s.Clear();
            s.Add("cat");
            s.Add("bird");
            s.Add("mouse");
            foreach(string i in s)
                report += i + " ";
            try
            {
                report += "\n Contains(\"bird\"): " + s.Contains("bird");
                report += "\n Contains(\"dinosaur\"): " + s.Contains("dinosaur");
                report += "\n Success";
            }
            catch (Exception)
            {
                report += " Fail";
            }

            return report;
        }

        public string tIEnum()
        {
            string report = "\nIEnumerator<T> Test:";
            report += "\n Set: ";
            Set<string> s = new Set<string>();
            s.Clear();
            s.Add("cat");
            s.Add("bird");
            s.Add("mouse");
            foreach (string i in s)
                report += i + " ";

           

            try
            {
                IEnumerator<string> ienum = s.GetEnumerator();
                report += "\n " + ienum.ToString();
                report += "\n Success";
            }
            catch (Exception)
            {
                report += " Fail";
            }

            return report;
        }

        
    }
}
