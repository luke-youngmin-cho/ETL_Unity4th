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
        float x;
        float y;
        float z;

        // 클래스 생성자
        // Manage Heap 메모리 영역에 이 클래스의 멤버변수들크기만큼 메모리를 할당후 생성된 객체의 주소참조를 반환함
        public CoordClass()
        {
        }

        public float Magnitude()
        {
            return x * x + y * y + z * z;
        }
    }

    internal class Dummy
    {
        public int poo1;
        public int poo2;
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


        }

        static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
    }
}