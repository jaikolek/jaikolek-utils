using System;
using UnityEngine;

namespace JaikolekUtils.CustomEnum
{
    [System.Serializable]
    public struct StringEnum
    {
        [SerializeField]
        [Tooltip("Type associated with this enum value.")]
        public string name;

        [SerializeField]
        [Tooltip("String value of the enum.")]
        public string value;

        // Constructor with default type (0)
        public StringEnum(string value)
        {
            this.name = "None"; // Default type
            this.value = value;
        }

        // Constructor with specified type and value
        public StringEnum(string type, string value)
        {
            this.name = type;
            this.value = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        }

        public static implicit operator string(StringEnum stringEnum) => stringEnum.value;
        public static implicit operator StringEnum(string value) => new StringEnum(value);
        public static implicit operator StringEnum((string name, string value) tuple) => new StringEnum(tuple.name, tuple.value);

        public override string ToString() => $"Type: {name}, Value: {value}";
    }
}
