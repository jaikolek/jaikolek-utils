using System;
using System.Collections;
using UnityEngine.Events;

namespace JaikolekUtils.Times
{
    public static class CoroutineUtil
    {
        public static IEnumerator DelayCoroutine(float delay, Action action = null)
        {
            yield return WaitForUtil.Seconds(delay);
            if (action != null) action.Invoke();
        }

        public static IEnumerator DelayCoroutine(int delay, Action action = null)
        {
            yield return WaitForUtil.Seconds(delay);
            if (action != null) action.Invoke();
        }

        public static IEnumerator DelayCoroutineRealtime(float delay, Action action = null)

        {
            yield return WaitForUtil.SecondsRealtime(delay);
            if (action != null) action.Invoke();
        }

        public static IEnumerator DelayCoroutineRealtime(int delay, Action action = null)
        {
            yield return WaitForUtil.SecondsRealtime(delay);
            if (action != null) action.Invoke();
        }
    }
}
