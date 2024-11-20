using EditorCustom.Attributes;
using Game.Events;
using Game.UI.Overlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ProjectInstallers
{
    public class OverlayInstaller : MonoInstaller
    {
        #region fields & properties
        [SerializeField] private ScreenFade screenFade;
        [SerializeField] private OverlayPanelsController overlayPanelsController;
        #endregion fields & properties

        #region methods
        public override void InstallBindings()
        {
            InstallScreenFade();
            InstallOverlayPanels();
        }
        private void InstallScreenFade()
        {
            Container.BindInterfacesAndSelfTo<ScreenFade>().FromInstance(screenFade).AsSingle();
            Container.QueueForInject(screenFade);
        }
        private void InstallOverlayPanels()
        {
            Container.BindInterfacesAndSelfTo<OverlayPanelsController>().FromInstance(overlayPanelsController).AsSingle();
            Container.QueueForInject(overlayPanelsController);
        }
        #endregion methods

#if UNITY_EDITOR
        [Title("Tests")]
        [SerializeField][DontDraw] private bool ___testBool;
        [Button(nameof(TestShowInfoRequest))]
        private void TestShowInfoRequest()
        {
            new InfoRequest(null, "header", "main text").Send();
        }
        [Button(nameof(TestShowConfirmRequest))]
        private void TestShowConfirmRequest()
        {
            new ConfirmRequest(delegate { Debug.Log("Confirm"); }, null, "header", "main text").Send();
        }
#endif //UNITY_EDITOR

    }
}