using UnityEngine;

namespace JaikolekUtils.Attributes
{
    public class NoteAttribute : PropertyAttribute
    {
        public string Text;

        public NoteAttribute(string text)
        {
            Text = text;
        }
    }

    [System.Serializable]
    public class NoteOnly { }
}
