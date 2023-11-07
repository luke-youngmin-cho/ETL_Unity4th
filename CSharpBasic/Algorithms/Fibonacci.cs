using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    internal class Fibonacci
    {
        public int this[int n]
        {
            get
            {
                if (n < 0)
                    throw new IndexOutOfRangeException();

                if (n > _limit)
                    return Get(n);

                return Cache[n];
            }
        }
        public int[] Cache;
        private int _limit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"> 피보나치 수열 항 </param>
        public Fibonacci(int n)
        {
            Get(n);
        }

        public int Get(int n)
        {
            _limit = n;
            Cache = new int[n + 1];
            return F(n);
        }

        // F(n) = F(n - 1) + F(n - 2)
        private int F(int n)
        {
            // 1이 수열 최솟값
            if (n <= 1)
                return n;

            // 이미 캐싱된 데이터 있으면 다시 계산 안함
            if (Cache[n] > 0)
                return Cache[n];

            Cache[n] = F(n - 1) + F(n - 2);
            return Cache[n];
        }
    }
}
