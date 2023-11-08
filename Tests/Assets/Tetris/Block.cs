namespace Test.Testris
{
    public abstract class Block
    {
        public Coord[] coords => patterns[_currentPattern];

        public int originX;
        public int originY;
        public Coord[][] patterns;
        public int currentPattern
        {
            get => _currentPattern;
            set
            {
                if (value == _currentPattern)
                    return;

                if (value >= patterns.Length)
                    value = 0;
                else if (value < 0)
                    value = patterns.Length - 1;

                _currentPattern = value;
            }
        }
        private int _currentPattern;

        public bool CW()
        {
            currentPattern++;
            return true;
        }
        public bool CCW()
        {
            currentPattern--;
            return true;
        }
    }

    public class BlockL : Block
    {
        public BlockL()
        {
        }
    }


}