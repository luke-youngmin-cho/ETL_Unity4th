namespace DynamicType
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = 1; // 컴파일시 a 변수는 int 타입으로 결정
            dynamic b = 2; // 런타임중 대입 값에따라 타입결정

            while (true)
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "int":
                        b = 2;
                        break;
                    case "float":
                        b = 3.0f;
                        break;
                    case "string":
                        b = "안녕";
                        break;
                    default:
                        break;
                }

                Console.WriteLine(b.GetType());
            }
        }
    }
}