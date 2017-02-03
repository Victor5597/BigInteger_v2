using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
            Matrix3x2 d = new Matrix3x2(1, 0, 0, 1, 1, 1);
            Console.WriteLine(d.GetDeterminant());
            while (true)
            {
                //Console.WriteLine(BigInt.DecToBin(new BigInt(Console.ReadLine(), 10)).ToString());
            }


        }
    }
}
