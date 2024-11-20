using Game.Serialization;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Universal.Behaviour;
using Universal.Core;
using Universal.Time;

namespace Game.UI.Overlay
{
    [System.Serializable]
    public class ScreenFade : IDisposable, Zenject.IInitializable
    {
        #region fields & properties
        public static UnityAction<bool> OnBlackScreenFading;
        public static UnityAction OnBlackScreenFadeUp;
        public static UnityAction OnBlackScreenFadeDown;
        public static readonly float FadeTime = 1f;
        [SerializeField] private CanvasAlphaChanger canvasAlphaChanger;
        #endregion fields & properties

        #region methods
        public void DoCycle() => SingleGameInstance.Instance.StartCoroutine(canvasAlphaChanger.DoCycle());
        public void Initialize()
        {
            canvasAlphaChanger.FadeDown();
            SavingController.OnDataReset += canvasAlphaChanger.HideCanvas;
            SceneLoader.OnStartLoading += OnSceneLoading;
            SceneLoader.OnSceneLoaded += canvasAlphaChanger.FadeDown;
        }
        public void Dispose()
        {
            SavingController.OnDataReset -= canvasAlphaChanger.HideCanvas;
            SceneLoader.OnStartLoading -= OnSceneLoading;
            SceneLoader.OnSceneLoaded -= canvasAlphaChanger.FadeDown;
        }
        private void OnSceneLoading(float offsetTime)
        {
            canvasAlphaChanger.Fade(true, 1f / offsetTime);
        }
        #endregion methods
    }
}