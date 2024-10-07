using System;
using Custom.Utils.Timer;

namespace TapTheMatch
{
    internal class TimerCtrl
    {
        private Action onTimeUpCallback;
        private Action<float> onTimerUpdatedCallback;
        private SimpleTimer simpleTimer;

        public TimerCtrl(Action onTimeUpCallback, Action<float> onTimerUpdatedCallback)
        {
            this.onTimeUpCallback = onTimeUpCallback;
            this.onTimerUpdatedCallback = onTimerUpdatedCallback;
        }

        public void StartTimer(float duration)
        {
            simpleTimer = new SimpleTimer();
            simpleTimer.StartTimer(duration, timer =>
            {
                onTimerUpdatedCallback?.Invoke(timer.normalized);
            }, onTimeUpCallback);
        }

        public void StopTimer()
        {
            simpleTimer.StopTimer();
        }

        public float GetNormalizedRemainingTime()
        {
            return simpleTimer.normalized;
        }
    }
}