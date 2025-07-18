using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils
{
    /// <summary>
    /// Parent object helping for scriptableobject script
    /// </summary>
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        /// <summary>
        /// Stands for "instance".
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<T>(typeof(T).ToString());
                }

                return instance;
            }
        }
    }
}
