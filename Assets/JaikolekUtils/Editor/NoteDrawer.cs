using UnityEditor;
using UnityEngine;

namespace JaikolekUtils.Attributes
{
    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class NoteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            NoteAttribute note = (NoteAttribute)attribute;

            // Get height of the field
            float fieldHeight = EditorGUI.GetPropertyHeight(property, label, true);

            // Define rects
            Rect fieldRect = new Rect(position.x, position.y, position.width, fieldHeight);
            Rect noteRect = new Rect(position.x, position.y + fieldHeight + 2, position.width, EditorGUIUtility.singleLineHeight * 2);

            // Draw field first
            EditorGUI.PropertyField(fieldRect, property, label, true);

            // Draw note below
            EditorGUI.HelpBox(noteRect, note.Text, MessageType.Info);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float fieldHeight = EditorGUI.GetPropertyHeight(property, label, true);
            float noteHeight = EditorGUIUtility.singleLineHeight * 2;
            return fieldHeight + 2 + noteHeight;
        }
    }
}
