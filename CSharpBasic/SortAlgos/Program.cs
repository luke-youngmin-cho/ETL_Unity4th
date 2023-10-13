using System.Diagnostics;
using System.Linq; // 다양한 자료구조들에 대한 탐색/취합/추출/반복 등에 대한 기능을 제공하는 namespace

namespace SortAlgos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int[] arr = //{ 1, 4, 3, 3, 9, 8, 7, 2, 5, 0 };
                Enumerable.Repeat(0, 10000000)
                          .Select(x => random.Next(0, 1000))
                          .ToArray();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //ArraySort.BubbleSort(arr); // 1만개 250ms, 100만개 270만ms
            //ArraySort.SelectionSort(arr); // 1만개 100ms
            //ArraySort.InsertionSort(arr); // 1만개 60ms, 100만개 55만ms
            //ArraySort.RecursiveMergeSort(arr); // 중복많을때: 1000만개 2000ms, 중복적을때: 1000만개 2500ms
            //ArraySort.MergeSort(arr); // 중복많을때: 1000만개 2000ms // 중복적을때: 1000만개 2400ms
            //ArraySort.RecursiveQuickSort(arr); // 중복많을때: 1000만개 187000ms, 중복적을때: 1000만개 1600ms 
            //ArraySort.QuickSort(arr);
            ArraySort.HeapSort(arr); // 중복많을때: 1000만개 3400ms, 중복적을때: 1000만개 3900ms 
            stopwatch.Stop();
            Console.WriteLine($"ElapsedTime : {stopwatch.ElapsedMilliseconds}");

            /*
            Console.Write("{");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr[i]}, ");
            }
            Console.Write("}");
            */
        }
    }

    internal class Dummy : IComparable<Dummy>
    {
        private float x, y;

        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public int CompareTo(Dummy? other)
        {
            throw new NotImplementedException();
        }

        // 연산자 오버로딩
        public static bool operator >=(Dummy a, Dummy b)
            => a.Magnitude() >= b.Magnitude();

        public static bool operator <=(Dummy a, Dummy b)
            => a.Magnitude() <= b.Magnitude();

        public static bool operator >(Dummy a, Dummy b)
            => a.Magnitude() > b.Magnitude();

        public static bool operator <(Dummy a, Dummy b)
            => a.Magnitude() < b.Magnitude();
    }

}