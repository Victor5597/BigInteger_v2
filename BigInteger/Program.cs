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
            List<BigInt> l = new List<BigInt>();
            l.Add(new BigInt(4));
            l.Add(new BigInt(2));
            l.Add(new BigInt(3));
            l = TimSort<BigInt>.TSort(l).ToList();
            for (int i = 0; i < l.Count; i++)
            {
                Console.Write(l[i].ToString() + " ");
            }
            while (true)
            {
                //Console.WriteLine(BigInt.DecToBin(new BigInt(Console.ReadLine(), 10)).ToString());
            }


        }
    }
}
