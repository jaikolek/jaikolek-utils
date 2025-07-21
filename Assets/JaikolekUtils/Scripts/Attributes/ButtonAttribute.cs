#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : Attribute
    {
        public string Label { get; }
        public float SpaceBefore { get; }

        public ButtonAttribute(string label = null, float spaceBefore = 0f)
        {
            Label = label;
            SpaceBefore = spaceBefore;
        }
    }
}
#endif