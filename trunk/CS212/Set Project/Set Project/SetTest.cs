using System;
using System.Collections.Generic;
using System.Text;

namespace SetProject
{
    class SetTest
    {
        
        public object run()
        {
            Set<string> s = new Set<string>();
            return(s.Clone());

        }
    }
}
