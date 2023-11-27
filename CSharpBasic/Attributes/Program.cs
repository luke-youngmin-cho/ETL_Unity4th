#define test
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

// Attribute (특성)
// Meta data 를 정의할때 사용하는 클래스

namespace Attributes
{
    [Obsolete("왠만하면 쓰지말고, 대신에 B 써라")]
    internal class A
    {
        [Obsolete()] // 더이상 사용되지 않는다는걸 뜻하는 attribute
        public void OldMethod() => Console.WriteLine("This is old");
        public void NewMethod() => Console.WriteLine("This is new");
    }

    internal class B
    {
        // Conditional
        // #define 전처리기에서 정의된 문자열일때만 구현하는 특성
        [Conditional("test")]
        public static void Log([CallerMemberName] string caller = default) 
            => Console.WriteLine($"I'm B, called by {caller}");
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.OldMethod();
            a.NewMethod();
            B.Log();

            LegoStore legoStore = new LegoStore();
            legoStore.PropertyChanged += (sender, args) =>
            {
                Console.WriteLine($"{args.PropertyName} of {sender} has changed..!");

                switch (args.PropertyName)
                {
                    case "CreatorTotal":
                        Console.WriteLine($"Luke 는 LegoStore 로 달려가기 시작했다...!");
                        break;
                    case "StarwarsTotal":
                        {
                            if (legoStore.StarwarsTotal < 3)
                                Console.WriteLine($"Luke : (아.. 하나라도 남아있다면좋겠다)");
                            else
                                Console.WriteLine($"Luke : (스타워즈는 다음에 사지 뭐");
                        }
                        break;
                    case "CityTotal":
                        Console.WriteLine("Luke : 아 또 City네...");
                        break;
                    default:
                        break;
                }

            };
            legoStore.StarwarsTotal = 3;
            legoStore.CreatorTotal = 3;
            legoStore.CityTotal = 3;


            GoldUI ui = new GoldUI();

            while (true)
            {
                string input = Console.ReadLine();

                GoldViewModel.Instance.Value = Int32.Parse(input);

                Console.WriteLine(ui.text);
            }
        }
    }
}