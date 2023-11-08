namespace Test.Testris
{
    public class Map
    {
        public int[,] nodes;
        public Block current;

        public bool MoveDown()
        {
            int expectedOriginY = current.originY + 1;
            int expectedOriginX = current.originX;

            for (int i = 0; i < current.coords.Length; i++)
            {
                Coord expected = current.coords[i];
                if (nodes[expected.y, expected.x] > 0)
                {
                    return false;
                }
            }

            current.originY = expectedOriginY;
            current.originX = expectedOriginX;
            return true;
        }

        public void CheckValid()
        {

        }
    }
}