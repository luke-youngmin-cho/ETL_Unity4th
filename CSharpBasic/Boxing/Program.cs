namespace Boxing
{
    class Dummy { }
    enum State { None, }
    internal class Program
    {
        // 이 함수의 결과는 어떤 정수값을 넣던 false.
        // 매개변수에 인자를 전달하는 순간 박싱이 일어나면서 새로운 객체가 만들어지기때문
        private static bool CompareNum(object num)
        {
            object _num = 3;
            return _num == num;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(CompareNum(3));
            int a = 3;
            object obj1 = 1; // boxing
            object obj2 = 1.4;
            object obj3 = "Luke";
            object obj4 = new Dummy();
            object obj5 = State.None;
            a = (int)obj1; // unboxing
            double d1 = (double)obj2;
            string name = (string)obj3;
            Dummy dummy = (Dummy)obj4;
            State state = (State)obj5;
            object obj6 = 8;
            float f = (float)obj2;
        }

    }
}