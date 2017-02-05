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
            b = TimSort<BigInt>.TSort(b).ToList();
            for (int i = 0; i < b.Count; i++)
            {
                Console.WriteLine(b[i].ToString() + "\t" );
            }
            Console.ReadKey();


        }
    }
}
