using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaikolekUtils.Anim
{
    public static class AnimationUtil
    {
        public static float ToSecond(this float frame)
        {
            return frame / 30f;
        }

        public static float ToFrame(this float second)
        {
            return second * 30f;
        }
    }
}
