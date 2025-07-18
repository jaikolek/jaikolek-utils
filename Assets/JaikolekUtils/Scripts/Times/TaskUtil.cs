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
            for (int i = delay; i >= 0; i++)
            {
                await Task.Delay(1000);
            }

            action?.Invoke();
        }

        public static async Task DelayAwaitAsync(float delay, Action action = null)
        {
            for (float i = delay; i >= 0; i -= 0.1f)
            {
                await Task.Delay((int)(.1f * 1000f));
            }

            action?.Invoke();
        }
    }
}
