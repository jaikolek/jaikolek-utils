using System;
using System.Threading.Tasks;

namespace JaikolekUtils.Times
{
    public static class TaskUtil
    {
        public static async void DelayAsync(int delay, Action action = null)
        {
            await DelayAwaitAsync(delay, action);
        }

        public static async void DelayAsync(float delay, Action action = null)
        {
            await DelayAwaitAsync(delay, action);
        }

        public static async Task DelayAwaitAsync(int delay, Action action = null)
        {
            await Task.Delay(1000 * delay);
            action?.Invoke();
        }

        public static async Task DelayAwaitAsync(float delay, Action action = null)
        {
            await Task.Delay((int)(1000f * delay));
            action?.Invoke();
        }
    }
}
