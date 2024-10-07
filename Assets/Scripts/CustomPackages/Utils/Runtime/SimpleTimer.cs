using UnityEngine;
using System;

namespace Custom.Utils.Timer
{
	public class SimpleTimer
	{
		public float life { get { return _life; } private set { _life = value; } }
		public float elapsed { get { return _curTime; } }
		public float normalized { get { return remaining / life; } } // returns timer as a range between 0 and 1
		public float remaining { get { return life - elapsed; } }
		public bool isFinished { get { return elapsed >= life; } }
		public bool isPaused { get { return _isPaused; } private set { _isPaused = value; } }

		protected bool _fixedTime;
		protected bool _isPaused;
		protected float _life;
		protected float _startTime;
		protected float _pauseTime;
		protected float _curTime { get { return (isPaused ? _pauseTime : _getTime) - _startTime; } set { _pauseTime = value; } }
		protected float _getTime { get { return _fixedTime ? Time.fixedTime : Time.realtimeSinceStartup; } }


		private Coroutine _everyFrameCorotuine;
		private Coroutine _timerCompletionCorotuine;

		public SimpleTimer()
		{
		}

		/// <summary>
		/// timer is implicitly started on instantiation
		/// </summary>
		/// <param name="lifeSpan">length of the timer</param>
		/// <param name="useFixedTime">use fixed (physics) time or screen update time</param>
		private SimpleTimer(float lifeSpan, bool useFixedTime = false)
		{
			life = lifeSpan;
			_fixedTime = useFixedTime;
			_startTime = _getTime;
		}

		/// <summary>
		/// starts timer again using time remaining 
		/// </summary>
		private void Resume() { _startTime = (isPaused ? _getTime - elapsed : _getTime); isPaused = false; }

		/// <summary>
		/// stop pauses the timer and allows for resume at current elapsed time
		/// </summary>
		private void Stop()
		{
			if (!isPaused)
			{
				_curTime = _getTime;
				isPaused = true;
			}
		}

		/// <summary>
		/// Add time to the timer
		/// </summary>
		/// <param name="amt"></param>
		public void AddTime(float amt)
		{
			_life += amt;
		}

		public void StartTimer(float lifeSpan, Action<SimpleTimer> onTimerUpdated, Action onTimerCompletion)
		{
			life = lifeSpan;
			_startTime = _getTime;
			StartTimerCorotuines(onTimerUpdated, onTimerCompletion);
			return;
		}

		public void StartTimerWithOffset(float lifeSpan, float remainingTime, Action<SimpleTimer> onTimerUpdated, Action onTimerCompletion)
		{
			LogUtils.Log("called: StartTimerWithOffset with lifeSpan, remainingTime: ", lifeSpan.ToString(), remainingTime.ToString());
			life = lifeSpan;
			_startTime = _getTime - (lifeSpan - remainingTime);
			StartTimerCorotuines(onTimerUpdated, onTimerCompletion);
			return;
		}

		private void StartTimerCorotuines(Action<SimpleTimer> onTimerUpdated, Action onTimerCompletion)
		{
			if (onTimerUpdated != null)
			{
				_everyFrameCorotuine = CoroutineUtils.Instance.CallEveryFrame(() => onTimerUpdated(this));
			}

			_timerCompletionCorotuine = CoroutineUtils.Instance.WaitUntilConditionIsMet(
			   () => this.isFinished,
			   () =>
			   {
				   CoroutineUtils.Instance.StopCoroutineCustom(_everyFrameCorotuine);
				   onTimerCompletion?.Invoke();
			   });
		}

		public void StopTimer()
		{
			Stop();

			CoroutineUtils.Instance.StopCoroutineCustom(_everyFrameCorotuine);
			CoroutineUtils.Instance.StopCoroutineCustom(_timerCompletionCorotuine);
		}
	}
}