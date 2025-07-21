using System;

namespace JaikolekUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StaticInspectableAttribute : Attribute
    {
        public string DisplayName { get; }

        public StaticInspectableAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}
