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
                string methodKey = GetMethodKey(targetType, method);

                GUILayout.Space(buttonAttr.SpaceBefore);

                string label = string.IsNullOrEmpty(buttonAttr.Label)
                    ? ObjectNames.NicifyVariableName(method.Name)
                    : buttonAttr.Label;

                if (!methodParamsCache.TryGetValue(methodKey, out var args) || args.Length != parameters.Length)
                {
                    args = new object[parameters.Length];
                    for (int i = 0; i < args.Length; i++)
                        args[i] = GetDefault(parameters[i].ParameterType);
                    methodParamsCache[methodKey] = args;
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    string paramKey = $"{target.GetInstanceID()}__{methodKey}__{param.Name}";
                    args[i] = DrawParameterField(paramKey, param.ParameterType, args[i], param.Name);
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

        private string GetMethodKey(Type type, MethodInfo method)
        {
            string paramTypes = string.Join("_", Array.ConvertAll(method.GetParameters(), p => p.ParameterType.Name));
            return $"{type.FullName}.{method.Name}__{paramTypes}";
        }

        private object DrawParameterField(string key, Type type, object currentValue, string labelOverride = null)
        {
            string label = ObjectNames.NicifyVariableName(labelOverride ?? key);

            switch (type)
            {
                case Type t when t == typeof(int):
                    int intVal = SessionState.GetInt(key, currentValue is int i ? i : 0);
                    intVal = EditorGUILayout.IntField(label, intVal);
                    SessionState.SetInt(key, intVal);
                    return intVal;

                case Type t when t == typeof(float):
                    float floatVal = SessionState.GetFloat(key, currentValue is float f ? f : 0f);
                    floatVal = EditorGUILayout.FloatField(label, floatVal);
                    SessionState.SetFloat(key, floatVal);
                    return floatVal;

                case Type t when t == typeof(string):
                    string strVal = SessionState.GetString(key, currentValue as string ?? "");
                    strVal = EditorGUILayout.TextField(label, strVal);
                    SessionState.SetString(key, strVal);
                    return strVal;

                case Type t when t == typeof(bool):
                    bool boolVal = SessionState.GetBool(key, currentValue is bool b && b);
                    boolVal = EditorGUILayout.Toggle(label, boolVal);
                    SessionState.SetBool(key, boolVal);
                    return boolVal;

                case Type t when t.IsEnum:
                    string enumString = SessionState.GetString(key, (currentValue ?? Enum.GetValues(type).GetValue(0)).ToString());
                    Enum enumVal = (Enum)Enum.Parse(type, enumString);
                    enumVal = EditorGUILayout.EnumPopup(label, enumVal);
                    SessionState.SetString(key, enumVal.ToString());
                    return enumVal;

                case Type t when typeof(UnityEngine.Object).IsAssignableFrom(t):
                    string objKey = key + "_objID";
                    int objID = SessionState.GetInt(objKey, (currentValue as UnityEngine.Object)?.GetInstanceID() ?? 0);
                    UnityEngine.Object obj = EditorUtility.InstanceIDToObject(objID);
                    obj = EditorGUILayout.ObjectField(label, obj, type, true);
                    SessionState.SetInt(objKey, obj ? obj.GetInstanceID() : 0);
                    return obj;

                case Type t when t.IsClass && t.IsSerializable:
                    object objInstance = currentValue ?? Activator.CreateInstance(t);
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(t.Name), EditorStyles.boldLabel);
                    foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        string fieldKey = key + "." + field.Name;
                        object fieldValue = field.GetValue(objInstance);
                        object updated = DrawParameterField(fieldKey, field.FieldType, fieldValue, field.Name);
                        field.SetValue(objInstance, updated);
                    }
                    EditorGUILayout.EndVertical();
                    return objInstance;

                default:
                    EditorGUILayout.LabelField($"Unsupported param type: {type.Name}");
                    return null;
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
            if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null) return Activator.CreateInstance(type);
            return null;
        }
    }
}