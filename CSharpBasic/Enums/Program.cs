namespace Enums
{
    // Bit flags
    // 1 -> 1
    // 10 -> 1*2^3 + 0*2^2 + 1*2^1 + 0*2^0 = 1010
    // 45 -> 2*16^1 + D*16^0
    // 45 -> 4*10^1 + 5*10^0
    public enum LayerMask
    {
        None    = 0 << 0, // ... 00000000
        Player  = 1 << 0, // ... 00000001
        Enemy   = 1 << 1, // ... 00000010
        Ground  = 1 << 2, // ... 00000100
        Wall    = 1 << 3, // ... 00001000
        GroundOrWall = Ground | Wall, // ... 00001100 -> Ground, Wall 만 검출됨
    }
    // ... 00000100
    // ... 00001100 &
    // ... 00000100 > 0


    public enum LayerMaskWrong
    {
        None,   // ... 00000000
        Player, // ... 00000001
        Enemy,  // ... 00000010
        Ground, // ... 00000011
        Wall,   // ... 00000100
        GroundOrWall = Ground | Wall, // ... 00000111 -> Player, Enemy, Ground, Wall 전부 검출됨
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return MathF.Sqrt((v1.x * v1.x - v2.x * v2.x) + (v1.y * v1.y - v2.y * v2.y));
        }
    }

    public class Collider
    {
        public string Name;
        public int Layer; // 자기 레이어
        public LayerMask LayerMask; // 충돌 감지할 레이어 마스크
        public Vector3 Position;

        public Collider(string name, int layer, LayerMask layerMask)
        {
            Name = name;
            Layer = layer;
            LayerMask = layerMask;
        }

        public void CollisionStay(Collider other)
        {
            // ... 00000010
            // ... 00001100 &
            // ... 00000100 == 4 > 0
            if ((1 << other.Layer & (int)this.LayerMask) > 0)
            {
                Console.WriteLine($"{this.Name} collided with {other.Name}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Collider player = new Collider("zl존검사", 0, LayerMask.Ground | LayerMask.Wall);
            Collider enemy = new Collider("빨강슬라임", 1, LayerMask.Ground | LayerMask.Wall);
            Collider ground = new Collider("땅", 2, LayerMask.Player | LayerMask.Enemy);
            Collider Wall = new Collider("벽", 3, LayerMask.Player | LayerMask.Enemy);

            player.CollisionStay(ground);

        }
    }
}