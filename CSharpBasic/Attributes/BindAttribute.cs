using System.Reflection;

namespace Attributes
{
    internal enum SourceTag
    {
        Gold,
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class BindAttribute : Attribute
    {
        public BindAttribute(string propertyName, SourceTag tag)
        {
            this.PropertyName = propertyName;
            this.Tag = tag;
        }


        public string PropertyName
        {
            get;
            private set;
        }

        public SourceTag Tag
        {
            get;
            private set;
        }
    }
}
