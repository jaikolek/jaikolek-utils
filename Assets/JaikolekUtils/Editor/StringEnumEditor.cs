using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ZLinq;

namespace JaikolekUtils.CustomEnum
{
    [CustomPropertyDrawer(typeof(StringEnum))]
    public class StringEnumEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(position, label.text);

            // Divide the position rect into three parts: label, name dropdown, and value dropdown
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = (position.width - labelWidth) * 0.5f; // Split the remaining space for two fields
            Rect nameRect = new Rect(position.x + labelWidth, position.y, fieldWidth, position.height);
            Rect valueRect = new Rect(position.x + labelWidth + fieldWidth + 5, position.y, fieldWidth - 5, position.height);

            // Get the current name and value properties
            SerializedProperty nameProperty = property.FindPropertyRelative("name");
            SerializedProperty valueProperty = property.FindPropertyRelative("value");

            // Extract the current values
            string currentName = nameProperty.stringValue;
            string currentValue = valueProperty.stringValue;

            // Find all instances of CustomEnumData in resources
            List<CustomEnumData> allDatas = AssetDatabase.FindAssets("t:CustomEnumData")
                .AsValueEnumerable()
                .Select(guid => AssetDatabase.LoadAssetAtPath<CustomEnumData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(data => data != null)
                .ToList();

            List<string> nameList = new List<string>();

            // Populate the name dropdown list
            foreach (CustomEnumData data in allDatas)
            {
                if (data.customEnumList != null)
                {
                    foreach (CustomEnum customEnum in data.customEnumList)
                    {
                        nameList.Add(customEnum.name);
                    }
                }
            }

            // Ensure there is at least one option for the name dropdown
            if (nameList.Count == 0)
            {
                nameList.Add("None");
            }

            // Determine the selected index for the name dropdown
            int nameSelectedIndex = nameList.IndexOf(currentName);
            if (nameSelectedIndex < 0) nameSelectedIndex = 0;

            // Draw the name dropdown
            nameSelectedIndex = EditorGUI.Popup(nameRect, nameSelectedIndex, nameList.ToArray());

            // Update the name property with the selected option
            string selectedName = nameList[nameSelectedIndex];
            nameProperty.stringValue = selectedName;

            // Populate the value dropdown list based on the selected name
            List<string> valueList = new List<string>();
            foreach (CustomEnumData data in allDatas)
            {
                if (data.customEnumList != null)
                {
                    foreach (CustomEnum customEnum in data.customEnumList)
                    {
                        if (customEnum.name == selectedName)
                        {
                            valueList.AddRange(customEnum.values);
                        }
                    }
                }
            }

            // Ensure there is at least one option for the value dropdown
            if (valueList.Count == 0)
            {
                valueList.Add("None");
            }

            // Determine the selected index for the value dropdown
            int valueSelectedIndex = valueList.IndexOf(currentValue);
            if (valueSelectedIndex < 0) valueSelectedIndex = 0;

            // Draw the value dropdown
            valueSelectedIndex = EditorGUI.Popup(valueRect, valueSelectedIndex, valueList.ToArray());

            // Update the value property with the selected option
            valueProperty.stringValue = valueList[valueSelectedIndex];

            EditorGUI.EndProperty();
        }
    }
}
