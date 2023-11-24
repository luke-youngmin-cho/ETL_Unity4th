namespace Attributes
{
    internal class GoldUI
    {
        [Bind("Value", SourceTag.Gold)]
        public string text
        {
            get;
            private set;
        }
    }
}
