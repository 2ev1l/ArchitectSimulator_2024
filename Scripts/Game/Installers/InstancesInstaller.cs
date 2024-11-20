using Game.Cameras;
using Game.Environment;
using Game.Player;
using Game.UI.Overlay;
using Game.UI.Overlay.Computer.DesignApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.Installers
{
    public class InstancesInstaller : MonoInstaller
    {
        #region fields & properties
        [SerializeField] private Player.Input playerInput;
        [SerializeField] private UIStateMachine uiStateMachine;
        [SerializeField] private CinemachineCamerasController camerasController;
        [SerializeField] private Crosshair crosshair;
        [SerializeField] private BlueprintEditor blueprintEditor;
        #endregion fields & properties

        #region methods
        public override void InstallBindings()
        {
            InstallPlayerInput();
            InstallUIStateMachine();
            InstallCamerasController();
            InstallCrosshair();
            InstallBlueprintEditor();
        }
        private void InstallUIStateMachine()
        {
            Container.Bind<UIStateMachine>().FromInstance(uiStateMachine).AsSingle().NonLazy();
            uiStateMachine.ForceInitialize();
            Container.QueueForInject(uiStateMachine);
        }
        private void InstallBlueprintEditor()
        {
            Container.Bind<BlueprintEditor>().FromInstance(blueprintEditor).AsSingle().NonLazy();
            blueprintEditor.ForceInitialize();
            Container.QueueForInject(blueprintEditor);
        }
        private void InstallPlayerInput()
        {
            Container.Bind<Player.Input>().FromInstance(playerInput).AsSingle().NonLazy();
            Container.QueueForInject(playerInput);
        }
        private void InstallCamerasController()
        {
            Container.Bind<CinemachineCamerasController>().FromInstance(camerasController).AsSingle().NonLazy();
            Container.QueueForInject(camerasController);
        }
        private void InstallCrosshair()
        {
            Container.Bind<Crosshair>().FromInstance(crosshair).AsSingle().NonLazy();
            Container.QueueForInject(crosshair);
        }

        #endregion methods
    }
}