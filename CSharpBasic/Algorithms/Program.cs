namespace Algorithms
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Fibonacci f = new Fibonacci(30);

            Console.WriteLine(f[3]);
            Console.WriteLine(f.Get(40));
        }

    }
}