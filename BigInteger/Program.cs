using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BigInteger
{
    class Fourier
    {
        public static void FFT(ref Complex[] Data)
        {
            if (Data.Length == 1) return;
            int N = 1 << (int)Math.Ceiling(Math.Log(Data.Length) / Math.Log(2));
            var l = new List<Complex>(Data);
            l.InsertRange(Data.Length, new Complex[N - Data.Length]);
            Data = l.ToArray();
            FFT_rec(ref Data, false);
        }
        public static void IFFT(ref Complex[] Data)
        {
            FFT_rec(ref Data, true);
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = Data[i] / Data.Length;
            }
        }
        static void FFT_rec(ref Complex[] data, bool IsInverseTransform)
        {
            if (data.Length <= 1) return;
            Complex[] dataEven = new Complex[data.Length / 2];
            Complex[] dataOdd = new Complex[data.Length / 2];
            for (int i = 0; i < data.Length;)
            {
                dataEven[i / 2] = data[i++];
                dataOdd[i / 2] = data[i++];
            }
            FFT_rec(ref dataEven, IsInverseTransform);
            FFT_rec(ref dataOdd, IsInverseTransform);
            for (int i = 0; i < data.Length / 2; ++i)
            {
                Complex phi = new Complex(Math.Cos(-2 * Math.PI * i / data.Length), Math.Sin(-2 * Math.PI * i / data.Length));
                Complex t = dataOdd[i] * (IsInverseTransform ? Complex.Conjugate(phi) : phi);
                data[i] = dataEven[i] + t;
                data[data.Length / 2 + i] = dataEven[i] - t;
            }
        }
        public static void Print(Complex[] Data)
        {
            Console.WriteLine(string.Join(" ", (from c in Data
                                                select Math.Round(c.Real)).ToArray()));
        }
    }
    class BigInt
    {
        public string s { get; private set; }
        public bool IsNeg { get; private set; }
        public int Radix { get; private set; }
        public BigInt(int num)
        {
            if (num < 0) IsNeg = true;
            s = Reverse(Math.Abs(num).ToString());
            s = RemoveZeros(s);
        }
        public BigInt(bool IsNegative, string str, bool IsReversed, int Rad = 10)
        {
            IsNeg = IsNegative;
            if (IsReversed) s = str;
            else s = Reverse(str);
            s = RemoveZeros(s);
            Radix = Rad;
        }
        public BigInt(string str, int Rad = 10)
        {
            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                IsNeg = true;
            }
            else IsNeg = false;
            Radix = Rad;
            s = Reverse(str);
            s = RemoveZeros(s);
        }
        public BigInt(BigInt n)
        {
            s = n.s;
            IsNeg = n.IsNeg;
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string RemoveZeros(string str)
        {
            while (str.Last() == '0' && str.Length > 1) str = str.Remove(str.Length - 1);
            return str;
        }
        
        public static BigInt Sum(BigInt A, BigInt B)
        {
            string s = "";
            BigInt a, b;
            if (A > B)
            {
                a = A;
                b = B;
            }
            else
            {
                a = B;
                b = A;
            }
            a.s += '0';
            b.s += new string('0', a.s.Length - b.s.Length);
            int cur = 0;
            for (int i = 0; i < a.s.Length; i++)
            {
                cur = a.s[i] - '0' + b.s[i] - '0' + cur;
                s += (char)('0' + cur % A.Radix);
                cur /= A.Radix;
            }
            s = RemoveZeros(s);
            return new BigInt(A.IsNeg, s, true, A.Radix);
        }
        public static BigInt Sub(BigInt A, BigInt B)
        {
            BigInt a, b;
            string s = "";
            bool IsNegative;
            if (A >= B)
            {
                a = A;
                b = B;
                IsNegative = A.IsNeg;
            }
            else
            {
                a = B;
                b = A;
                IsNegative = B.IsNeg;
            }
            b.s += new string('0', a.s.Length - b.s.Length);
            byte count = 0;
            for (int i = 0; i < a.s.Length; i++)
            {
                if (a.s[i] - b.s[i] - count >= 0)
                {
                    s += (char)(a.s[i] - b.s[i] - count + '0');
                    count = 0;
                }
                else
                {
                    s += (char)(a.s[i] - b.s[i] - count + '0' + A.Radix);
                    count = 1;
                }
            }
            s = s.Insert(b.s.Length, a.s.Substring(b.s.Length));
            s = RemoveZeros(s);
            return new BigInt(IsNegative, s, true);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is BigInt)) return false;
            if (obj as BigInt != this) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(BigInt A, BigInt B)
        {
            if ((A.s == B.s) && (A.IsNeg == B.IsNeg)) return true;
            else return false;
        }
        public static bool operator !=(BigInt A, BigInt B)
        {
            return !(A == B);
        }
        public static bool operator >(BigInt A, BigInt B)
        {
            if (A.s.Length > B.s.Length)
            {
                return true;
            }
            else
            {
                if (A.s.Length < B.s.Length) return false;
                for (int i = A.s.Length - 1; i >= 0; i--)
                {
                    if (A.s[i] > B.s[i]) return true;
                    if (A.s[i] < B.s[i]) return false;
                }
                return false;
            }
        }
        public static bool operator <(BigInt A, BigInt B)
        {
            if (A > B) return false;
            if (A == B) return false;
            return true;
        }
        public static bool operator >=(BigInt A, BigInt B)
        {
            if (A < B) return false;
            return true;
        }
        public static bool operator <=(BigInt A, BigInt B)
        {
            if (A > B) return false;
            return true;
        }
        public static BigInt operator +(BigInt A, BigInt B)
        {
            if (A.Radix != B.Radix) throw new Exception();
            if (A.IsNeg == B.IsNeg)
            {
                return Sum(A, B);
            }
            else
            {
                return Sub(A, B);
            }
        }
        public static BigInt operator +(BigInt A, int b)
        {
            return A + new BigInt(b);
        }
        public static BigInt operator -(BigInt A)
        {
            A.IsNeg = !A.IsNeg;
            return A;
        }
        public static BigInt operator -(BigInt A, BigInt B)
        {
            return (A + (-B));
        }
        public Complex[] FFT(int length)
        {
            Complex[] form = new Complex[length];
            for (int i = length - 1; i >= length - s.Length; i--)
            {
                form[i] = (char)s[length - i - 1] - '0';
            }
            Fourier.FFT(ref form);
            return form;
        }
        public static BigInt operator *(BigInt A, double d)
        {
            if (d == A.Radix)
            {
                if (A.s == "0") return A;
                A.s = A.s.Insert(0, "0");
                return A;
            }
            else return new BigInt(0);
        }
        public static BigInt operator *(BigInt A, BigInt B)
        {
            if (A.Radix != B.Radix) throw new Exception();
            Complex[] a, b;
            int N = 2 << (int)Math.Ceiling(Math.Log(Math.Max(A.s.Length, B.s.Length)) / Math.Log(2));
            a = A.FFT(N);
            b = B.FFT(N);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] *= b[i];
            }

            Fourier.IFFT(ref a);
            BigInt C = new BigInt(0);
            for (int i = 0; i < a.Length - 1; i++)
            {
                C = C * A.Radix + new BigInt((int)Math.Round(a[i].Real));
            }
            return C;
        }
        public static BigInt operator /(BigInt N, BigInt D)
        {
            if (N.Radix != D.Radix) throw new Exception();
            if (N < D) return new BigInt(0);
            int size = N.s.Length - D.s.Length + 1;
            int pow = D.s.Length;
            BigInt x = new BigInt(N.Radix / (D.s.Last() - '0'));
            x.s = new string('0', size) + x.s;
            BigInt two = new BigInt(2);
            two.s = new string('0', size + pow) + two.s;
            BigInt prev;
            while (true)
            {
                prev = x;
                x = x * (two - D * x);
                x.s = x.s.Remove(0, size + pow);
                if (x == prev) break;
            }
            BigInt Q = N * x;
            Q.s = Q.s.Remove(0, size + pow);
            if ((Q + 1) * D <= N) Q += 1;
            return Q;
        }
        public static BigInt operator %(BigInt N, BigInt D)
        {
            return new BigInt(N - (N / D) * D);
        }
        public void Print()
        {
            if (IsNeg) System.Console.Write('-');
            System.Console.WriteLine(Reverse(s));
        }
        public static BigInt DecToBin(BigInt a) {
            if (a.s.Length == 1) return new BigInt(Table_DecToBin(a.s[0]), 2);
            int N = 1 << (int)Math.Ceiling(Math.Log(a.s.Length) / Math.Log(2));
            a.s += new string('0', N - a.s.Length);
            BigInt a1 = new BigInt(a.s.Substring(0, a.s.Length / 2), 2);
            BigInt a2 = new BigInt(a.s.Substring(a.s.Length / 2), 2);
            //s = s1 + s2 * 10^length/2
            a1 = DecToBin(a1);
            a2 = DecToBin(a2);
            return Merge(a1, a2, a.s.Length/2);
        }
        static BigInt Merge(BigInt a, BigInt B, int shift)
        {
            BigInt A = new BigInt(a.s.Substring(shift), 2);
            a.s = a.s.Substring(0, shift);
            return new BigInt(a.s + (B + A).s, 2);
        }
        static string Table_DecToBin(char c)
        {
            switch (c)
            {
                case '0':
                    return "0";
                case '1':
                    return "1";
                case '2':
                    return "10";
                case '3':
                    return "11";
                case '4':
                    return "100";
                case '5':
                    return "101";
                case '6':
                    return "110";
                case '7':
                    return "111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                default:
                    throw new Exception();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                BigInt a = new BigInt(Console.ReadLine());
                //BigInt b = new BigInt(Console.ReadLine(), 2);
                a = BigInt.DecToBin(a);
                a.Print();
            }


        }
    }
}
