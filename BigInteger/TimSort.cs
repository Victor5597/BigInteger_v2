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
            int N = Data.Count;
            int minRun = 32;
            //add log(N/32) to improve evenness of arrays
            int n = 0;
            while(true)
            {
                if (N - n < minRun) run = Data.GetRange(n, N - n);
                else run = Data.GetRange(n, minRun);
                n += minRun;
                for (int i = 0; i < run.Count; i++)
                {
                    if (i == 0 || run[i].CompareTo(run[i - 1]) > 0) continue;
                    for(int j = 0; j < i; j++)
                    {
                        if (run[j].CompareTo(run[i]) < 0) continue;
                        T buff = run[i];
                        for (int k = i; k > j; k--)
                        {
                            run[k] = run[k-1];
                        }
                        run[j] = buff;
                        break;
                    }
                }

                break;//1 round
            }
            
            return run;
        }
    }
}
