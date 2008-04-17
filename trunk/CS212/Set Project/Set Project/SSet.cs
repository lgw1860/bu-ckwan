/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * SSet
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SetProject
{
    public class SSet : Set<string>, IDeserializationCallback
    {
        public SSet() : base()
        {
        }

        public SSet(int i) : base(i)
        {
        }

        protected SSet(SerializationInfo information, StreamingContext context)
            : base(information, context)
        {
        }

        public override void OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
        }

    }
}
