using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BigInteger
{
    static class Fourier
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
    }
    class BigInt
    {
        public string Data { get; private set; }
        public bool IsNeg { get; private set; }
        public readonly int Radix;
        static string[] table;
        public BigInt(long num, int Rad = 10)
        {
            if (num < 0) IsNeg = true;
            Data = Reverse(Math.Abs(num).ToString());
            Data = RemoveZeros(Data);
            Radix = Rad;
        }
        public static implicit operator BigInt(long n)
        {
            return new BigInt(n);
        }
        public BigInt(bool IsNegative, string str, bool IsReversed, int Rad = 10)
        {
            IsNeg = IsNegative;
            if (IsReversed) Data = str;
            else Data = Reverse(str);
            Data = RemoveZeros(Data);
            Radix = Rad;
        }
        public BigInt(string str, int Rad = 10)
        {
            if (str == "") str += "0";
            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                IsNeg = true;
            }
            else IsNeg = false;
            Radix = Rad;
            Data = Reverse(str);
            Data = RemoveZeros(Data);
        }
        public BigInt(BigInt n)
        {
            Data = n.Data;
            IsNeg = n.IsNeg;
            Radix = n.Radix;
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
        public BigInt Abs()
        {
            return (IsNeg ? -this : this);
        }
        static BigInt Sum(BigInt A, BigInt B)
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
            a.Data += '0';
            b.Data += new string('0', a.Data.Length - b.Data.Length);
            int cur = 0;
            for (int i = 0; i < a.Data.Length; i++)
            {
                cur = a.Data[i] - '0' + b.Data[i] - '0' + cur;
                s += (char)('0' + cur % A.Radix);
                cur /= A.Radix;
            }
            s = RemoveZeros(s);
            return new BigInt(A.IsNeg, s, true, A.Radix);
        }
        static BigInt Sub(BigInt A, BigInt B)
        {
            BigInt a, b;
            string s = "";
            bool IsNegative;
            if (A.Abs() >= B.Abs())
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
            b.Data += new string('0', a.Data.Length - b.Data.Length);
            byte count = 0;
            for (int i = 0; i < a.Data.Length; i++)
            {
                if (a.Data[i] - b.Data[i] - count >= 0)
                {
                    s += (char)(a.Data[i] - b.Data[i] - count + '0');
                    count = 0;
                }
                else
                {
                    s += (char)(a.Data[i] - b.Data[i] - count + '0' + A.Radix);
                    count = 1;
                }
            }
            s = s.Insert(b.Data.Length, a.Data.Substring(b.Data.Length));
            s = RemoveZeros(s);
            return new BigInt(IsNegative, s, true, A.Radix);
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
            return ((A.Data == B.Data) && (A.IsNeg == B.IsNeg));
        }
        public static bool operator !=(BigInt A, BigInt B)
        { 
            return !(A == B);
        }
        public static bool operator >(BigInt A, BigInt B)
        {
            if (A.IsNeg != B.IsNeg) return !A.IsNeg;
            if (A.Data.Length > B.Data.Length)
            {
                return !A.IsNeg;
            }
            else
            {
                if (A.Data.Length < B.Data.Length) return A.IsNeg;
                for (int i = A.Data.Length - 1; i >= 0; i--)
                {
                    if (A.Data[i] > B.Data[i]) return !A.IsNeg;
                    if (A.Data[i] < B.Data[i]) return A.IsNeg;
                }
                return false;
            }
        }
        public static bool operator <(BigInt A, BigInt B)
        {
            return !(A > B || A == B);
        }
        public static bool operator >=(BigInt A, BigInt B)
        {
            return !(A < B);
        }
        public static bool operator <=(BigInt A, BigInt B)
        {
            return !(A > B);
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
        public static BigInt operator -(BigInt A)
        {
            return new BigInt(!A.IsNeg, A.Data, true, A.Radix);
        }
        public static BigInt operator -(BigInt A, BigInt B)
        {
            if (A.Radix != B.Radix) throw new Exception();
            return (A + (-B));
        }
        Complex[] FFT(int length)
        {
            Complex[] form = new Complex[length];
            for (int i = length - 1; i >= length - Data.Length; i--)
            {
                form[i] = (char)Data[length - i - 1] - '0';
            }
            Fourier.FFT(ref form);
            return form;
        }
        
        public static BigInt operator *(BigInt A, BigInt B)
        {
            if (A.Radix != B.Radix) throw new Exception();
            Complex[] a, b;
            int N = 2 << (int)Math.Ceiling(Math.Log(Math.Max(A.Data.Length, B.Data.Length)) / Math.Log(2));
            a = A.FFT(N);
            b = B.FFT(N);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] *= b[i];
            }
            Fourier.IFFT(ref a);
            BigInt C = new BigInt("0", A.Radix);
            for (int i = 0; i < a.Length - 1; i++)
            {
                C.Data = C.Data == "0" ? C.Data : C.Data = C.Data.Insert(0, "0");
                C += new BigInt((int)Math.Round(a[i].Real), C.Radix);
            }
            return (A.IsNeg ^ B.IsNeg) ? -C : C;
        }
        public static BigInt operator /(BigInt N, BigInt D)
        {
            if (D == 0) throw new DivideByZeroException();
            if (N.Radix != D.Radix) throw new Exception();
            bool IsNegative = N.IsNeg ^ D.IsNeg;
            N = N.Abs();
            D = D.Abs();
            if (N < D) return new BigInt(0, N.Radix);
            int size = N.Data.Length - D.Data.Length + 1;
            int pow = D.Data.Length;
            BigInt x = BigInt.DecToBin(N.Radix / (D.Data.Last() - '0'), N.Radix);
            x.Data = new string('0', size) + x.Data;
            BigInt two = BigInt.DecToBin(2, N.Radix);
            two.Data = new string('0', size + pow) + two.Data;
            BigInt prev;
            while (true)
            {
                prev = x;
                x = x * (two - D * x);
                x.Data = x.Data.Remove(0, size + pow);
                if (x == prev) break;
            }
            BigInt Q = N * x;
            Q.Data = Q.Data.Remove(0, size + pow);
            if ((Q + new BigInt(1, Q.Radix)) * D <= N) Q += new BigInt(1, Q.Radix);
            Q.Data = RemoveZeros(Q.Data);
            return IsNegative ? -Q : Q;
        }
        public static BigInt operator %(BigInt N, BigInt D)
        {
            return (N - (N / D) * D);
        }
        public override string ToString()
        {
            if (Data == "0") IsNeg = false;
            return (IsNeg ? "-" : "") + Reverse(Data);
        }
        public static BigInt DecToBin(BigInt a)
        {
            BigInt.Table_DecToBin_init();
            return BigInt.DecToBin_rec(a);
        }
        static BigInt DecToBin_rec(BigInt a) {
            
            if (a.Data.Length == 1) return new BigInt(a.IsNeg, BigInt.table[a.Data[0]- '0'], false, 2);
            int N = 1 << (int)Math.Ceiling(Math.Log(a.Data.Length) / Math.Log(2));
            a.Data += new string('0', N - a.Data.Length);
            BigInt a1 = new BigInt(false ,a.Data.Substring(0, a.Data.Length / 2), true, 10);
            BigInt a2 = new BigInt(false, a.Data.Substring(a.Data.Length / 2), true, 10);
            //Data = s1 + s2 * 10^length/2
            a1 = DecToBin_rec(a1);
            a2 = DecToBin_rec(a2);
            BigInt bin = Merge(a1, a2, a.Data.Length/2);
            return new BigInt(a.IsNeg, bin.Data, true, 2);
        }
        public static BigInt DecToBin(int dec, int rad)
        {
            string b = "";
            if (rad == 10) b = rad.ToString();
            else
            {
                while (dec != 0)
                {
                    b += (dec % rad).ToString();
                    dec /= rad;
                }
            }
            return new BigInt(dec < 0, b, true, rad);
        }
        static BigInt Merge(BigInt a, BigInt B, int shift)
        {
            /*BigInt pow = new BigInt("1", 2);
            for (int i = 0; i < shift; i++)
            {
                pow = pow * new BigInt("1010", 2);
            }*/
            return a + B * BigInt.FastPow(1010, shift, 2);
        }
        static public BigInt FastPow(int a, int pow, int rad)
        {
            BigInt A = new BigInt(a, rad);
            BigInt b = new BigInt(1, rad);
            BigInt map = BigInt.DecToBin(pow, 2);
            for (int i = map.Data.Length-1; i >= 0; i--)
            {
                b *= b;
                if (map.Data[i] == '1') b *= A;
            }
            return b;
        }
        static string[] Table_DecToBin_init()
        {
            table = new string[11];
            table[0] = "0";
            table[1] = "1";
            table[2] = "10";
            table[3] = "11";
            table[4] = "100";
            table[5] = "101";
            table[6] = "110";
            table[7] = "111";
            table[8] = "1000";
            table[9] = "1001";
            table[10] = "1010";
            return table;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(BigInt.DecToBin(new BigInt(Console.ReadLine(), 10)).ToString());
            }


        }
    }
}
