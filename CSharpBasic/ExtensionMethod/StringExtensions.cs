
namespace ExtensionMethod
{
    internal static class StringExtensions
    {
        public static int WordCount(this string source)
        {
            return source.Split(new char[] { ' ', '.', ',', '!', '?' }, 
                StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
