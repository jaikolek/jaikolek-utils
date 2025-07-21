using System;
using System.Collections.Generic;
using UnityEngine;
using ZLinq;
using JaikolekUtils.Attributes;

namespace JaikolekUtils.CustomEnum
{
    [CreateAssetMenu(fileName = "CustomEnumData", menuName = "Enum/Custom Enum Data")]
    public class CustomEnumData : ScriptableObjectSingleton<CustomEnumData>
    {
        [Note("Make sure you update parameter 'value' usage to, if you want updating this custom enum data")]
        public NoteOnly noteOnly;

        public List<CustomEnum> customEnumList;
        public Dictionary<string, string> customs;

        public int Count(string name)
        {
            foreach (CustomEnum costumEnum in customEnumList)
            {
                if (costumEnum.name.Equals(name))
                {
                    return costumEnum.values.Length;
                }
            }

            return 0;
        }

        public CustomEnum Get(string name)
        {
            return customEnumList
                .AsValueEnumerable()
                .FirstOrDefault(e => e.name.Equals(name));
        }

        public StringEnum GetEnum(string name, int index)
        {
            foreach (CustomEnum costumEnum in customEnumList)
            {
                if (costumEnum.name.Equals(name))
                {
                    if (index > Count(name) - 1) return null;

                    return new StringEnum(name, costumEnum.values[index]);
                }
            }

            return null;
        }

        public StringEnum Random(string name)
        {
            CustomEnum customEnum = Get(name);
            return new StringEnum(name, customEnum.values[UnityEngine.Random.Range(0, customEnum.values.Length)]);
        }

        public int GetEnumIndex(StringEnum stringEnum)
        {
            CustomEnum customEnum = Get(stringEnum.name);
            return Array.IndexOf(customEnum.values, stringEnum.value);
        }

        public List<StringEnum> GetList(string name)
        {
            List<StringEnum> list = new List<StringEnum>();
            CustomEnum customEnum = Get(name);

            foreach (string str in customEnum.values)
            {
                list.Add(new StringEnum(name, str));
            }

            return list;
        }

        public bool IsEnumAvailable(StringEnum stringEnum)
        {
            List<StringEnum> stringEnumList = GetList(stringEnum.name);

            return stringEnumList
                .AsValueEnumerable()
                .Any(e => e.Equals(stringEnum));
        }
    }

    [System.Serializable]
    public struct CustomEnum
    {
        public string name;
        public string[] values;
    }
}
