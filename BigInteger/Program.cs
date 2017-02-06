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
            List<BigInt> b = new List<BigInt>();
            Random r = new Random();
            for (int i = 0; i < 25; i++)
            {
                b.Add(new BigInt(r.Next()));
            }
            for (int i = 0; i < 25; i++)
            {
                l.Add(Math.Sin(i + 0.5));
            }
            //b = TimSort<BigInt>.TSort(b).ToList();
            l = TimSort<double>.TSort(l).ToList();
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i].ToString() + "\t" );
            }
            Console.ReadKey();


        }
    }
}
