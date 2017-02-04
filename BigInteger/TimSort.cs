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
            List<T> run = new List<T>();
            int N = Data.Count, minRun = 32, L;
            //add log(N/32) to improve evenness of arrays
            List<Tuple<int, int>> map = new List<Tuple<int, int>>();
            int n = 0;
            while(n != N)
            {
                int peak;
                for (peak = 0; peak < N - n - 1; peak++)
                {
                    if (peak == 0 || Data[n + peak].CompareTo(Data[n + peak - 1]) > 0) continue;
                    break;
                }
                if (N - (n + peak) < minRun || N - (n + minRun) < minRun)
                {
                    run = Data.GetRange(n, N - n);
                    n = N;
                }
                else
                {
                    if (peak >= minRun)
                    {
                        run = Data.GetRange(n, peak);
                        n += peak;
                        continue;
                    }
                    else
                    {
                        run = Data.GetRange(n, minRun);
                        n += minRun;
                    }
                }
                map.Add(new Tuple<int, int>(n, run.Count));
                for (; peak < run.Count; peak++)
                {
                    int Stage;
                    for (Stage = 0; Stage < peak; Stage++)
                    {
                        if (run[Stage].CompareTo(run[peak]) < 0) continue;
                        break;
                    }
                    T buff = run[peak];
                    for (int wrongStage = peak; wrongStage > Stage; wrongStage--)
                    {
                        run[wrongStage] = run[wrongStage - 1];
                    }
                    run[Stage] = buff;
                }
                for (int i = n - run.Count; i < n; i++)
                {
                    Data[i] = run[i];//opt!!!
                }
            }
            Stack<Tuple<int, int>> Addr_Len = new Stack<Tuple<int, int>>();
            while (true) {
                int i = 0;
                Addr_Len.Push(map[0]);
                if (map[0].Item1 > map[1].Item1 + map[2].Item1 && map[1].Item1 > map[2].Item1) {
                    continue;
                }
                else
                {
                    if (x < y) {
                        Merge(ref Data, map[0].Item1, map[0].Item2, map[1].Item1, map[1].Item2);
                        x = x + y;
                        y = z;
                        z = sizes.Dequeue();
                        my = mz;
                        mz = map.Dequeue();
                    }
                    else Merge(ref Data, map.Dequeue(), y, , z);
                }
            }
            return Data;
        }
        static void Merge(ref List<T> Data, int i1, int c1, int i2, int c2)
        {

        }
    }
}
