using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Behaviour;
using Universal.Core;
using Universal.Time;

namespace Game.UI.Overlay
{
    [System.Serializable]
    public class CanvasAlphaChanger
    {
        #region fields & properties
        public UnityAction OnFadeUp;
        public UnityAction OnFadeDown;
        public CanvasGroup FadeCanvas => fadeCanvas;
        [SerializeField] private CanvasGroup fadeCanvas;
        [SerializeField] private ValueTimeChanger fadeTimeChanger;
        public float LastFadingTime => lastFadingTime;
        private float lastFadingTime = 0f;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Hides immediately
        /// </summary>
        public void HideCanvas()
        {
            fadeCanvas.alpha = 0;
            fadeCanvas.gameObject.SetActive(false);
        }
        public void FadeDown() => Fade(false);
        public void FadeUp() => Fade(true);

        public IEnumerator DoCycle(float animationSpeed = 1f)
        {
            Fade(true, animationSpeed);
            yield return new WaitForSeconds(1f / animationSpeed);
            Fade(false, animationSpeed);
            yield return new WaitForSeconds(1f / animationSpeed);
        }
        public void Fade(bool fadeUp, float animationSpeed = 1f)
        {
            if (!fadeCanvas.gameObject.activeSelf)
                fadeCanvas.gameObject.SetActive(true);
            fadeCanvas.alpha = fadeUp ? 0 : 1;
            fadeCanvas.blocksRaycasts = fadeUp;
            int finalValue = fadeUp ? 1 : 0;
            ChangeAlpha(finalValue, 1f / animationSpeed);
            if (fadeUp) OnFadeUp?.Invoke();
            else OnFadeDown?.Invoke();
        }
        private void ChangeAlpha(int finalValue, float time)
        {
            lastFadingTime = time;
            bool up = (finalValue + 1) % 2 == 0;
            fadeTimeChanger.SetValues((finalValue + 1) % 2, finalValue);
            fadeTimeChanger.SetActions(x => fadeCanvas.alpha = x,
                delegate
                {
                    if (!up) HideCanvas();
                    else fadeCanvas.alpha = 1;
                });
            fadeTimeChanger.Restart(time);
        }
        public bool IsBlackScreenFade() => FadeCanvas.alpha > 0f && FadeCanvas.alpha < 1f;
        #endregion methods
    }
}