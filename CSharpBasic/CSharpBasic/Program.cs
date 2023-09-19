namespace CSharpBasic
{
    internal struct Coord
    {
        public float x;
        public float y;
        public float z;

        // 구조체생성자
        // 구조체의 데이터를 초기화 및 추가 연산 정의 가능
        public Coord()
        {
            x = 0;
            y = 0;
            z = 0;
        }
    }

    // C# 구조체 및 클래스는 캡슐화 컨셉.
    // -> 외부에서 함부로 멤버에 접근하면 안되기 때문에 기본적으로 접근제한자가 private 임.
    internal class CoordClass
    {
        // 싱글톤 패턴
        // 인스턴스를 참조할 static 변수를 사용하는 형태.
        // 일반적으로 인스턴스가 힙 영역에 하나만 존재할 경우 객체 참조 구조를 간단하게 하기위해서 사용함.
        // 모든 데이터 혹은 클래스 자체를 static 으로 하게될 경우, 사용되지 않아도 리소스가 메모리에 올라와야하기때문에
        // 사용하는 경우가 생겼을때만 리소스들 로드/생성 해서 메모리에 올려놓기 위한 형태
        public static CoordClass instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CoordClass();
                return _instance;
            }
        }
        private static CoordClass _instance;

        // static 멤버변수 : instance 할당시 포함되지않으므로 BSS/DATA 영역에 있는 데이터를 직접 읽고 씀
        public static float x = 1.0f; 

        // instance 멤버 변수 : instance 할당시 BSS/DATA 영역에 있는 데이터로 생성된
        // 인스턴스의 변수들을 초기화 하고, 그 인스턴스영역에 있는 데이터를 읽고 씀
        public float y =2.0f;
        public float z;


        // 클래스 생성자
        // Manage Heap 메모리 영역에 이 클래스의 멤버변수들크기만큼 메모리를 할당후 생성된 객체의 주소참조를 반환함
        public CoordClass()
        {
        }

        // this 키워드
        // 인스턴스 멤버 함수는 어떤 인스턴스의 데이터를 읽고 써야하는지 정해주기위해서 
        // 파라미터로 대상이될 인스턴스에 대한 참조가필요한데, 
        // 이를 코드에서 생략하고 this 키워드로 제공해줌. 
        // this 키워드도 인스턴스 대상으로는 생략할수있음
        public float Magnitude()
        {
            float result = x * x + y * this.y + this.z * this.z;
            return result;
        }
    }

    internal class Dummy
    {
        public int poo1;
        public int poo2;

        public void Test()
        {
            CoordClass coord = new CoordClass();
            CoordClass.Magnitude()
            Console.WriteLine(CoordClass.instance.Magnitude());
        }
    }


    internal class Program
    {
        // 값 vs 참조
        // 값 타입 : 메모리의 데이터를 직접 읽거나 쓰는 형태
        // 참조 타입 : 메모리의 주소를 통해 간접적으로 읽거나 쓰는 형태

        // 구조체 vs 클래스
        // 값 vs 참조
        // 값을 자주 읽고 쓰면서, 16 byte 이하이면서, 확장성에 대해 닫혀있을 때 구조체를 사용함.
        static void Main(string[] args)
        {
            int a = 3;
            Console.WriteLine(a);

            int[] arr = new int[2];
            Swap(ref arr[0],ref arr[1]);



            Coord coord = new Coord();
            Console.WriteLine(coord.x);
            CoordClass coordClass = new CoordClass();
            Console.WriteLine(coord.x);

            //Dummy dummy = new Dummy();
            // 주소참조변수는 모두 똑같이 주소를 저장하기위한 4byte만 필요하지만,
            // 간접참조시 얼마만큼의 데이터를 어떻게 읽어야 하는지 알아야 하기 때문에 참조변수 타입은 객체타입과 동일해야한다
            //dummy = new CoordClass();
            //coordClass.x

            Dummy dummy0 = new Dummy();
            Dummy dummy1 = new Dummy();
            Dummy dummy2 = new Dummy();
            Dummy dummy3 = new Dummy();
            Dummy dummy4 = new Dummy();
            dummy0.Test();
            dummy1.Test();
            dummy2.Test();
            dummy3.Test();
            dummy4.Test();
            Console.WriteLine(dummy4.poo1);
        }

        static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
    }
}