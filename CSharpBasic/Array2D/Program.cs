namespace Array2D
{
    internal class Program
    {
        static int[,] map = new int[4, 5]
        {
            { 3, 0, 0, 0, 0},
            { 0, 0, 1, 1, 1},
            { 1, 0, 0, 0, 0},
            { 1, 0, 1, 1, 2},
        };
        static int x, y; // 플레이어 좌표
        static int goalX = 4, goalY = 3; // 목표 좌표

        static void Main(string[] args)
        {
            DisplayMap();

            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (Move(key))
                {
                    DisplayMap();

                    if (x == goalX && y == goalY)
                        break;
                }
                else
                {
                    Console.WriteLine("그곳으로는 갈 수 없습니다.");
                }
            }
            Console.WriteLine("도착 ! 고생하셨습니다.");
        }

        static void DisplayMap()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                        Console.Write($"□");
                    else if (map[i, j] == 1)
                        Console.Write($"■");
                    else if (map[i, j] == 2)
                        Console.Write($"☆");
                    else if (map[i, j] == 3)
                        Console.Write($"▣");
                }
                Console.WriteLine();
            }
        }

        static bool Move(ConsoleKey key)
        {
            bool result = false;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    result = MoveLeft();
                    break;
                case ConsoleKey.UpArrow:
                    result = MoveUp();
                    break;
                case ConsoleKey.RightArrow:
                    result = MoveRight();
                    break;
                case ConsoleKey.DownArrow:
                    result = MoveDown();
                    break;
                default:
                    break;
            }
            return result;
        }


        static bool MoveRight()
        {
            // 맵의 경계를 벗어나는지
            if (x >= map.GetLength(1) - 1)
                return false;

            // 이동할수 있는 칸인지
            if (map[y, x + 1] == 1)
                return false;

            map[y, x] = 0;
            x++;
            map[y, x] = 3;
            return true;
        }

        static bool MoveLeft()
        {
            // 맵의 경계를 벗어나는지
            if (x <= 0)
                return false;

            // 이동할수 있는 칸인지
            if (map[y, x - 1] == 1)
                return false;

            map[y, x] = 0;
            x--;
            map[y, x] = 3;
            return true;
        }

        static bool MoveDown()
        {
            // 맵의 경계를 벗어나는지
            if (y >= map.GetLength(0) - 1)
                return false;

            // 이동할수 있는 칸인지
            if (map[y + 1, x] == 1)
                return false;

            map[y, x] = 0;
            y++;
            map[y, x] = 3;
            return true;
        }

        static bool MoveUp()
        {
            // 맵의 경계를 벗어나는지
            if (y <= 0)
                return false;

            // 이동할수 있는 칸인지
            if (map[y - 1, x] == 1)
                return false;

            map[y, x] = 0;
            y--;
            map[y, x] = 3;
            return true;
        }
    }
}