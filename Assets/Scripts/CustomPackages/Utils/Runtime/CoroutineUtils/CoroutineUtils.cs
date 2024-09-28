using System;
using System.Collections;
using UnityEngine;

namespace Custom.Utils
{
    public class CoroutineUtils : MonoBehaviour
    {
        public static CoroutineUtils instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        #region EVERY FRAME
        public Coroutine CallEveryFrame(Action callBack)
        {
            return StartCoroutine(EveryFrameCorotine(callBack));
        }
        private IEnumerator EveryFrameCorotine(Action callBack)
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                callBack?.Invoke();
            }
        }
        #endregion

        #region BASED ON CONDITION 
        public Coroutine WaitUntillContionIsMet(Func<bool> conditionFunc, Action callBackOnConditionIsMet)
        {
            return StartCoroutine(WaitUntillConditionIsMet(conditionFunc, callBackOnConditionIsMet));
        }

        private IEnumerator WaitUntillConditionIsMet(Func<bool> conditionFunc, Action callBackOnConditionIsMet)
        {
            yield return new WaitUntil(() => conditionFunc.Invoke());
            callBackOnConditionIsMet?.Invoke();
        }
        #endregion

        #region BASED ON TIME 
        public Coroutine WaitUntillGivenTime(float waitTimetoCall, Action callBackOnTimeCompletion)
        {
            return StartCoroutine(WaitUntillGivenTimeCoroutine(waitTimetoCall, callBackOnTimeCompletion));
        }

        private IEnumerator WaitUntillGivenTimeCoroutine(float waitTimetoCall, Action callBackOnTimeCompletion)
        {
            yield return new WaitForSeconds(waitTimetoCall);
            callBackOnTimeCompletion?.Invoke();
        }
        #endregion

        public void StopCorotuineCustom(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}