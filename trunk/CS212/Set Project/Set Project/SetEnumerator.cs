/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * SetEnumerator
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections; //for IEnumerator properties

namespace SetProject
{
    public class SetEnumerator<T> : IEnumerator<T>
    {
        Dictionary<T, object>.KeyCollection.Enumerator iter;
        Set<T> s;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s_"></param>
        public SetEnumerator(Set<T> s_)
        {
            s = s_;
            iter = s.Keys.GetEnumerator();
        }

        #region IEnumerator<T> Members

        public T Current
        {
            get
            {
                return iter.Current;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return iter.Current;
            }
        }

        public bool MoveNext()
        {
            return iter.MoveNext();
        }

        public void Reset()
        {
            iter = s.Keys.GetEnumerator();
        }

        #endregion

        #region extra methods from spec

        bool IEnumerator.MoveNext()
        {
            return iter.MoveNext();
        }

        void IEnumerator.Reset()
        {
            iter = s.Keys.GetEnumerator();
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return iter.Current;
            }
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
