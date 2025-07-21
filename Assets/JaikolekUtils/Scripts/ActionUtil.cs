using System;

namespace JaikolekUtils
{
    /// <summary>
    /// This class provides utility methods for working with Action.
    /// You can add more code to this class as needed.
    /// </summary>
    public static class ActionUtils
    {
        public static void RemoveNullListeners(this Action action)
        {
            if (action == null) return;

            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null)
                {
                    action -= (Action)d;
                }
            }
        }

        public static void RemoveNullListeners(this Action<string> action)
        {
            if (action == null) return;

            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null)
                {
                    action -= (Action<string>)d;
                }
            }
        }

        public static void RemoveNullListeners(this Action<int> action)
        {
            if (action == null) return;

            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null)
                {
                    action -= (Action<int>)d;
                }
            }
        }

        public static void RemoveNullListeners(this Action<float> action)
        {
            if (action == null) return;

            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null)
                {
                    action -= (Action<float>)d;
                }
            }
        }

        public static void RemoveNullListeners(this Action<bool> action)
        {
            if (action == null) return;

            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null)
                {
                    action -= (Action<bool>)d;
                }
            }
        }
    }
}
