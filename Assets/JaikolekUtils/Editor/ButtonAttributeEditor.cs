#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JaikolekUtils.Attributes
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonAttributeEditor : Editor
    {
        private Dictionary<string, object[]> methodParamsCache = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (target == null) return;

            var targetType = target.GetType();
            var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var buttonAttr = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttr == null) continue;

                var parameters = method.GetParameters();
                string methodKey = method.Name;

                GUILayout.Space(buttonAttr.SpaceBefore);

                string label = string.IsNullOrEmpty(buttonAttr.Label)
                    ? ObjectNames.NicifyVariableName(method.Name)
                    : buttonAttr.Label;

                object[] args = new object[parameters.Length];
                if (!methodParamsCache.TryGetValue(methodKey, out args))
                {
                    args = new object[parameters.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = GetDefault(parameters[i].ParameterType);
                    }
                    methodParamsCache[methodKey] = args;
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    Type type = param.ParameterType;

                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{param.Name} ({type.Name})", GUILayout.Width(150));

                    if (type == typeof(int))
                        args[i] = EditorGUILayout.IntField((int)args[i]);
                    else if (type == typeof(float))
                        args[i] = EditorGUILayout.FloatField((float)args[i]);
                    else if (type == typeof(string))
                        args[i] = EditorGUILayout.TextField((string)args[i]);
                    else if (type == typeof(bool))
                        args[i] = EditorGUILayout.Toggle((bool)args[i]);
                    else if (type.IsEnum)
                        args[i] = EditorGUILayout.EnumPopup((Enum)args[i]);
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                        args[i] = EditorGUILayout.ObjectField((UnityEngine.Object)args[i], type, true);
                    else
                        EditorGUILayout.LabelField($"Unsupported param type: {type.Name}");

                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button(label))
                {
                    try
                    {
                        method.Invoke(target, args);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[Button] Error invoking method '{method.Name}': {e}");
                    }
                }
            }
        }

        private object GetDefault(Type type)
        {
            if (type == typeof(string)) return "";
            if (type == typeof(int)) return 0;
            if (type == typeof(float)) return 0f;
            if (type == typeof(bool)) return false;
            if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
            if (typeof(UnityEngine.Object).IsAssignableFrom(type)) return null;
            return null; // default for unsupported types
        }
    }
}
#endif