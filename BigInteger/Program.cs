using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigInteger
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> l = new List<double>();
            l.Add(10);
            l.Add(-3);
            l.Add(5);
            for (int i = 0; i < 25; i++)
            {
                l.Add(Math.Sin(i));
            }
            l = TimSort<double>.TSort(l).ToList();
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i].ToString() + " ");
            }
            Console.ReadKey();


        }
    }
}
