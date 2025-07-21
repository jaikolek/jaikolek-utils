using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JaikolekUtils.Attributes
{
    public class StaticClassInspectorWindow : EditorWindow
    {
        private Type selectedType;
        private Vector2 scroll;

        [MenuItem("Window/Static Class Inspector")]
        public static void ShowWindow()
        {
            GetWindow<StaticClassInspectorWindow>("Static Class Inspector");
        }

        private void OnGUI()
        {
            DrawTypeSelector();

            if (selectedType == null)
            {
                EditorGUILayout.HelpBox("Select a static class to inspect.", MessageType.Info);
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawStaticFieldsAndProperties(selectedType);
            DrawStaticMethods(selectedType);
            EditorGUILayout.EndScrollView();
        }

        private void DrawTypeSelector()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.IsAbstract && t.IsSealed && t.GetCustomAttribute<StaticInspectableAttribute>() != null)
                .ToList();

            var names = types.Select(t => t.GetCustomAttribute<StaticInspectableAttribute>()?.DisplayName ?? t.Name).ToArray();

            int index = selectedType != null ? types.IndexOf(selectedType) : -1;
            int newIndex = EditorGUILayout.Popup("Static Class", index, names);

            if (newIndex != index)
                selectedType = newIndex >= 0 ? types[newIndex] : null;
        }

        private void DrawStaticFieldsAndProperties(Type type)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.CanRead && p.CanWrite);

            EditorGUILayout.LabelField("Fields", EditorStyles.boldLabel);

            foreach (var field in fields)
            {
                object value = field.GetValue(null);

                EditorGUI.BeginChangeCheck();
                object newValue = DrawField(field.Name, value, field.FieldType);
                if (EditorGUI.EndChangeCheck())
                {
                    field.SetValue(null, newValue);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);

            foreach (var prop in props)
            {
                object value = prop.GetValue(null);

                EditorGUI.BeginChangeCheck();
                object newValue = DrawField(prop.Name, value, prop.PropertyType);
                if (EditorGUI.EndChangeCheck())
                {
                    prop.SetValue(null, newValue);
                }
            }
        }

        private object DrawField(string label, object value, Type type)
        {
            if (type == typeof(int))
                return EditorGUILayout.IntField(label, (int)value);
            if (type == typeof(float))
                return EditorGUILayout.FloatField(label, (float)value);
            if (type == typeof(bool))
                return EditorGUILayout.Toggle(label, (bool)value);
            if (type == typeof(string))
                return EditorGUILayout.TextField(label, (string)value);
            if (type.IsEnum)
                return EditorGUILayout.EnumPopup(label, (Enum)value);

            EditorGUILayout.LabelField(label, $"Unsupported type: {type.Name}");
            return value;
        }

        private void DrawStaticMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetParameters().Length == 0 && m.ReturnType == typeof(void));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Methods", EditorStyles.boldLabel);

            foreach (var method in methods)
            {
                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(null, null);
                }
            }
        }
    }
}
