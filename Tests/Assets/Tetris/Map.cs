namespace Test.Testris
{
    public static class Map
    {
        public static int[,] nodes;
        public static Block current;

        public static bool TryMove(Coord dir)
        {
            Coord expectedOrigin = current.origin + dir;

            for (int i = 0; i < current.localCoords.Length; i++)
            {
                Coord expected = expectedOrigin + current.localCoords[i];
                if (nodes[expected.y, expected.x] > 0)
                {
                    return false;
                }
            }

            current.origin = expectedOrigin;
            return true;
        }

    }
}