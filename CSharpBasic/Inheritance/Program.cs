namespace Inheritance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Character character = new Character();
            Warrior warrior1 = new Warrior();
            warrior1.NickName = "지존검사"; // setter 호출 (인자는 "지존검사")
            Console.WriteLine($"{warrior1.NickName} 의 경험치 : {warrior1.Exp}"); // NickName 과 Exp 의 getter 호출
            Goblin goblin1 = new Goblin();


            // 공변성
            // 하위타입을 기반타입으로 참조할 수 있는 성질
            Character player = warrior1;
            player.Breath();
            IHp target = goblin1;

            player.Attack(target);
            
            if (player is Warrior)
            {
                Console.WriteLine(((Warrior)player).anger);
            }

            // 데이터의 승격 (Promotion)
            // 데이터를 읽는 과정에서 상위 용량의 데이터형식으로 변하는 현상
            int int1 = 1;
            long long1 = int1;

            long long2 = 1;
            int int2 = (int)long2;

        }
    }
}