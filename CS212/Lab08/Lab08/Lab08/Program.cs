using System;
using System.Collections.Generic;
using System.Text;

namespace Lab08
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] values = { 3.0, 5.0, 6.0, 7.0, 1.0};
            sbyte[] tieBreakValues = { 2, 3, 1, 4, 5};

            Mord m = new Mord(values,tieBreakValues);
            //m.run();
            Console.WriteLine("\nBigGood obtains: ");
            m.GetMord(Mord.eDirection.BigGood);
            m.GetKord(tieBreakValues,Mord.eDirection.BigGood);

            Console.WriteLine("\n\n\nLittleGood obtains: ");
            m.GetMord(Mord.eDirection.LittleGood);
            m.GetKord(tieBreakValues, Mord.eDirection.LittleGood);

            Console.ReadLine();
            
        }
    }
}
