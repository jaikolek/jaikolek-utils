using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils.Anim
{
    public class DynamicAnimationOnEnable : DynamicAnimation
    {
        protected virtual void OnEnable()
        {
            Play();
        }

        protected virtual void OnDisable()
        {
            Stop();
        }
    }
}
