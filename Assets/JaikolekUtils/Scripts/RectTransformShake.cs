using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils
{
    public class RectTransformShake
    {
        private readonly RectTransform target;
        private readonly Vector2 originalPos;
        private readonly List<(float duration, float strength, float elapsed)> shakes = new();

        public Coroutine shakeCoroutine;

        public RectTransformShake(RectTransform target)
        {
            this.target = target;
            this.originalPos = target.anchoredPosition;
        }

        public IEnumerator DoShake(float duration, float strength)
        {
            shakes.Add((duration, strength, 0f));

            while (shakes.Count > 0)
            {
                Vector2 offset = Vector2.zero;

                for (int i = shakes.Count - 1; i >= 0; i--)
                {
                    (float d, float s, float e) = shakes[i];
                    e += Time.deltaTime;

                    if (e >= d)
                    {
                        shakes.RemoveAt(i);
                        continue;
                    }

                    shakes[i] = (d, s, e);

                    float t = 1f - (e / d);
                    offset += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * s * t;
                }

                target.anchoredPosition = originalPos + offset;
                yield return null;
            }

            target.anchoredPosition = originalPos;
            shakeCoroutine = null;
        }
    }
}
