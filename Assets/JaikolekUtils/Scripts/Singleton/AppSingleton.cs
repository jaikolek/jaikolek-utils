using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils.Singleton
{
    /// <summary>
    /// Parent object helping for singleton script and don't destroy on load
    /// </summary>
    public abstract class AppSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
                    instance = (T)FindObjectOfType(typeof(T));
                    DontDestroyOnLoad(instance.gameObject);

                    Debug.Log($"{typeof(T)} force to initialize.");
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else if (!instance.gameObject.Equals(this.gameObject))
            {
                Debug.LogWarning($"{typeof(T)} already exists.");
                Destroy(this.gameObject);

                return;
            }
        }

        protected virtual void Initialize()
        {

        }
    }
}
