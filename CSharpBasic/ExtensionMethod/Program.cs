using System.Linq;

namespace ExtensionMethod
{
    public class A 
    {
        public int a;
    }
    public static class B
    {
        public static int b;
        public static void Test1()
        {
            //Console.WriteLine(b);
        }

        // 확장함수는 어떤 객체의 기능을 외부에서 추가할 때 사용
        // 인터페이스는 객체 자신의 기능을 추가할 때 상속하여 사용
        public static void Test2(this A target) // this 의 의미는 멤버접근연산자의 왼쪽 객체참조를 받는 키워드
        {
            Console.WriteLine(target.a);
        }

        public static int Test4<T>(this T target)
        {
            Console.WriteLine(target.GetType());
            return -1;
        }

        public static void Test3()
        {
            Console.WriteLine(b);
        }
    }

    public class Dummy
    {
        void Main()
        {
            A a = new A();
            a.Test2();
        }
    }


    public class ListExtension<T> : List<T>
    {
        public IEnumerable<T> Where(Predicate<T> predicate)
        {
            List<T> list = new List<T>();
            foreach (T item in this)
                if (predicate(item))
                    list.Add(item);
            return list;
        }
    }

    // 확장 메서드 
    // 어떤 객체참조를 대상으로 기능을 확장할 때 사용하는 함수 
    // static 클래스내에서 static 메서드를 만들고 기능을확장해야하는 객체참조를 파라미터로 받는다.


    internal class Program
    {
        static void Main(string[] args)
        {
            string name = "Luke Cho";
            Console.WriteLine(name.WordCount());

            StringExtensions.WordCount(name);
            name.WordCount();
            
            List<int> numbers = new List<int>()
            {
                1,2,3,6,2,3,4,10,44,22,33,55,22,77,42,12,
            };

            IEnumerable<string> filtered =
            from number in numbers
            where number > 5
            orderby number descending
            select $"number : {number}";

            foreach (string number in filtered)
                Console.WriteLine(number);

            IEnumerable<string> filtered2 =
            numbers.Where(x => x > 5)
                   .OrderByDescending(x => x)
                   .Select(x => $"number : {x}");

            IEnumerable<int> numbersFiltered =
                numbers.Where(x => x > 5);

            LinkedList<int> linkedList = new LinkedList<int>();
            linkedList.Where(x => x > 5);
        }
    }
}