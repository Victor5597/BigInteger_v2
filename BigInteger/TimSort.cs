using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigInteger
{
    static class TimSort<T> where T : IComparable
    {
        public static IEnumerable<T> TSort(IEnumerable<T> DataIn)
        {
            List<T> Data = DataIn.ToList();
            Stack<Tuple<int, int>> map = RunSplitter(ref Data);
            MergeAll(ref Data, ref map);
            return Data;
            
        }
        static Stack<Tuple<int,int>> RunSplitter(ref List<T> Data)
        {
            int N = Data.Count;
            Stack<Tuple<int, int>> map = new Stack<Tuple<int, int>>();
            int n = 0;
            while (n != N)
            {
                int peak = FindNextRun(ref Data, n, ref map);
                for (; peak < map.Peek().Item2; peak++)
                {
                    int Stage;
                    for (Stage = 0; Stage < peak; Stage++)
                    {
                        if (Data[n + Stage].CompareTo(Data[n + peak]) < 0) continue;
                        break;
                    }
                    T buff = Data[n + peak];
                    for (int wrongStage = peak; wrongStage > Stage; wrongStage--)
                    {
                        Data[n + wrongStage] = Data[n + wrongStage - 1];
                    }
                    Data[n + Stage] = buff;
                }
                n += map.Peek().Item2;
            }
            return map;
        }
        static int FindNextRun(ref List<T> Data, int n, ref Stack<Tuple<int, int>> map)
        {
            int max, minRun = 8;//optimize
            //add log(N/32) to improve evenness of arrays
            for (max = 0; max < Data.Count - n - 1; max++)
            {
                if (max == 0 || Data[n + max].CompareTo(Data[n + max - 1]) > 0) continue;
                break;
            }
            int RunCount;
            if (Data.Count - (n + max) < minRun || Data.Count - (n + minRun) < minRun)
            {
                RunCount = Data.Count - n;
            }
            else
            {
                if (max >= minRun)
                {
                    RunCount = max;
                }
                else
                {
                    RunCount = minRun;
                }
            }
            map.Push(new Tuple<int, int>(n, RunCount));
            return max;
        }
        static void MergeAll(ref List<T> Data, ref Stack<Tuple<int, int>> map)
        {
            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            if (map.Count >= 3)
            {
                Tuple<int, int> x = map.Pop();
                Tuple<int, int> y = map.Pop();
                Tuple<int, int> z = map.Pop();
                while (true)
                {
                    if (x.Item1 > y.Item1 + z.Item1 && y.Item1 > z.Item1)
                    {
                        stack.Push(x);
                        x = y;
                        y = z;
                    }
                    else
                    {
                        if (x.Item2 > z.Item2)
                        {
                            y = Merge(ref Data, z, y);
                        }
                        else
                        {
                            x = Merge(ref Data, y, x);
                            y = z;
                        }
                    }
                    if (map.Count != 0) z = map.Pop();
                    else
                    {
                        stack.Push(x);
                        stack.Push(y);
                        break;
                    }
                }
            }
            else
            {
                if (map.Count == 2) stack.Push(map.Pop());
                stack.Push(map.Pop());
            }
            Tuple<int, int> current = stack.Pop();
            while (stack.Count != 0)
            {
                current = Merge(ref Data, current, stack.Pop());
            }
        }
        static Tuple<int, int> Merge(ref List<T> Data, Tuple<int,int> run1, Tuple<int,int> run2)
        {
            bool FromLeftMerge;
            FromLeftMerge = (run1.Item2 > run2.Item2) ? false : true;
            Tuple<int, int> tempInfo = FromLeftMerge ? run1 : run2;
            Tuple<int, int> secInfo = FromLeftMerge ? run2 : run1;
            T[] tempArr = new T[tempInfo.Item2];
            Data.CopyTo(tempInfo.Item1, tempArr, 0, tempInfo.Item2);//Why not "ref tempArr"?
            int i, si, ti;
            ti = i = FromLeftMerge ? 0 : tempInfo.Item2 - 1;
            si = FromLeftMerge ? 0 : secInfo.Item2 - 1;
            while(true)
            {
                if (ti >= tempArr.Length || si == secInfo.Item2 || ti < 0 || si < 0) break;
                bool TempIsNext = tempArr[ti].CompareTo(Data[secInfo.Item1 + si]) < 0;
                if (FromLeftMerge) Data[tempInfo.Item1 + i++] = TempIsNext ?
                        tempArr[ti++] : Data[secInfo.Item1 + si++];
                else Data[tempInfo.Item1 + i--] = !TempIsNext ?
                        tempArr[ti--] : Data[secInfo.Item1 + si--];                
            }
            if (si == secInfo.Item2 || si < 0)
            {
                while (i < tempInfo.Item2 + secInfo.Item2 && i >= -secInfo.Item2)
                {
                    if (FromLeftMerge) Data[tempInfo.Item1 + i++] = tempArr[ti++];
                    else Data[tempInfo.Item1 + i--] = tempArr[tempInfo.Item2 - 1 - ti--];
                }
            }
            return new Tuple<int, int>(run1.Item1, run1.Item2 + run2.Item2);
        }
    }
}
