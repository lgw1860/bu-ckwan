using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicString
{
    class MyString
    {
        int time = 5;
        String name = "Greg";

        public int Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
