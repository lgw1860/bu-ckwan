/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * STL set referenced from http://www.sgi.com/tech/stl/set.html
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SetProject
{
    [Serializable]
    class Set<T> : Dictionary<T, object>,
        IDeserializationCallback,
        ICloneable, IEnumerable<T>
    {

        Dictionary<T, object> theSet;
        private sbyte count = 0;

        /// <summary>
        /// Creates an empty set.
        /// </summary>
        public Set()
        {
            theSet = new Dictionary<T, object>();
        }

     
        /// <summary>
        /// Creates a set that can initially contain 'capacity' elements.
        /// </summary>
        /// <param name="capacity">initial number of elements set can contain</param>
        public Set(int capacity)
        {
            theSet = new Dictionary<T, object>(capacity);
        }


        /// <summary>
        /// Creates a set with 'IEqualityComparer ic' for comparing keys.
        /// </summary>
        /// <param name="ic">for comparing keys</param>
        public Set(IEqualityComparer<T> ic)
        {
            theSet = new Dictionary<T, object>(ic);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            Dictionary<T, object> clone = new Dictionary<T, object>();
            foreach (KeyValuePair<T, object> k in theSet)
            {
                clone.Add(k.Key, k.Value);
            }
            return clone;
        }

        /// <summary>
        /// Creates a set with the info required to serialize it.
        /// </summary>
        /// <param name="information"></param>
        /// <param name="context"></param>
        protected Set(SerializationInfo information,
            StreamingContext context) : base(information, context)
        {
            //theSet = new Dictionary<T, sbyte>(information, context);
            //theSet = base(information, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public override void OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
        }

        /// <summary>
        /// Values property - obtain the values in the set
        /// </summary>
        public new Dictionary<T, object>.KeyCollection Values
        {
            get
            {
                return theSet.Keys;
            }
        }

        /// <summary>
        /// Return a list of all elements in set.
        /// </summary>
        public List<T> List
        {
            get
            {
                List<T> l = new List<T>();
                foreach (KeyValuePair<T, object> k in theSet)
                {
                    l.Add(k.Key);
                }
                return l;
            }

        }

        /// <summary>
        /// if successful add, return true
        /// </summary>
        /// <param name="key">key to add to set</param>
        /// <returns>true if successful add, false if fail</returns>
        public bool Add(T key)
        {
            try
            {
                count++;
                theSet.Add(key, count);
                Console.WriteLine("{0}: {1}",key,theSet[key]);
                return true;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument Null Exception.  Cannot add null to the set");
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Argument Exception.  Please check argument and try again.");
                return false;
            }   
            catch (Exception)
            {
                Console.WriteLine("Exception. Please try again");
                return false;
            }
        }



        public new IEnumerator<T> GetEnumerator()
        {
            return null;
        }
    }
}
