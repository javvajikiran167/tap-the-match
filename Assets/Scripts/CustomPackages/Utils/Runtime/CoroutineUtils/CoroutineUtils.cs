using System;
using System.Collections;
using UnityEngine;

namespace Custom.Utils
{
    public class CoroutineUtils : MonoBehaviour
    {
        public static CoroutineUtils Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional: Keep the instance alive across scenes
            }
            else
            {
                Destroy(gameObject); // Optional: Destroy duplicate instances
            }
        }

        #region EVERY FRAME
        /// <summary>
        /// Calls the provided callback every frame.
        /// </summary>
        /// <param name="callback">The callback to be called every frame.</param>
        public Coroutine CallEveryFrame(Action callback)
        {
            return StartCoroutine(EveryFrameCoroutine(callback));
        }

        private IEnumerator EveryFrameCoroutine(Action callback)
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                callback?.Invoke();
            }
        }
        #endregion

        #region BASED ON CONDITION 
        /// <summary>
        /// Waits until the specified condition is met, then calls the provided callback.
        /// </summary>
        /// <param name="conditionFunc">The condition to be met.</param>
        /// <param name="callbackOnConditionMet">The callback to be called when the condition is met.</param>
        public Coroutine WaitUntilConditionIsMet(Func<bool> conditionFunc, Action callbackOnConditionMet)
        {
            return StartCoroutine(WaitUntilConditionIsMetCoroutine(conditionFunc, callbackOnConditionMet));
        }

        private IEnumerator WaitUntilConditionIsMetCoroutine(Func<bool> conditionFunc, Action callbackOnConditionMet)
        {
            yield return new WaitUntil(() => conditionFunc.Invoke());
            callbackOnConditionMet?.Invoke();
        }
        #endregion

        #region BASED ON TIME 
        /// <summary>
        /// Waits for the specified time, then calls the provided callback.
        /// </summary>
        /// <param name="waitTime">The time to wait.</param>
        /// <param name="callbackOnTimeCompletion">The callback to be called after the wait time.</param>
        public Coroutine WaitUntilGivenTime(float waitTime, Action callbackOnTimeCompletion)
        {
            return StartCoroutine(WaitUntilGivenTimeCoroutine(waitTime, callbackOnTimeCompletion));
        }

        private IEnumerator WaitUntilGivenTimeCoroutine(float waitTime, Action callbackOnTimeCompletion)
        {
            yield return new WaitForSeconds(waitTime);
            callbackOnTimeCompletion?.Invoke();
        }
        #endregion

        /// <summary>
        /// Stops the specified coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to stop.</param>
        public void StopCoroutineCustom(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}