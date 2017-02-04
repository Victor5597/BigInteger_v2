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
            l.Add(new BigInt(3));
            l.Add(new BigInt(1));
            l.Add(new BigInt(5));
            l.Sort();
            while (true)
            {
                //Console.WriteLine(BigInt.DecToBin(new BigInt(Console.ReadLine(), 10)).ToString());
            }
        }
    }
}
