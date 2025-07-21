using System.Collections;
using UnityEngine;

namespace JaikolekUtils
{
    public class TransformShake
    {
        readonly protected Transform transformShake;
        readonly protected float defaultAmmount;
        readonly protected float defaultDuration;
        readonly protected Vector2 originalPos;

        public float shakeElapsed = 0f;
        public float maxDuration = 0f;
        public float currentStrength = 0f;
        public Coroutine shakeCoroutine;

        public TransformShake(Transform transformShake, float defaultAmmount, float defaultDuration, Vector2 originalPos)
        {
            this.transformShake = transformShake;
            this.defaultAmmount = defaultAmmount;
            this.defaultDuration = defaultDuration;
            this.originalPos = originalPos;
        }

        public IEnumerator DoShake(float duration)
        {
            while (shakeElapsed < duration || currentStrength > .01f)
            {
                float strength = currentStrength * (1f - Mathf.Clamp01(shakeElapsed / duration));
                transformShake.position = originalPos + new Vector2(UnityEngine.Random.Range(-1f, 1f) * strength, UnityEngine.Random.Range(-1f, 1f) * strength);
                shakeElapsed += Time.deltaTime;
                currentStrength = Mathf.Lerp(currentStrength, 0f, Time.deltaTime * 5f);
                yield return null;
            }

            transformShake.position = originalPos;
            currentStrength = 0f;
            shakeElapsed = 0f;
            maxDuration = 0f;
            shakeCoroutine = null;
        }
    }
}
