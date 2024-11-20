using Game.Serialization.Settings;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Behaviour;
using Universal.Core;
using Universal.Serialization;
using QualitySettings = Universal.Serialization.QualitySettings;

namespace Game.UI.Overlay
{
    [System.Serializable]
    public class ScreenSettings : IDisposable, Zenject.IInitializable
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        public void Initialize()
        {
            Awake();
            SceneLoader.OnSceneLoaded += Awake;
            SettingsData.Data.OnGraphicsChanged += OnGraphicsChanged;
            SettingsData.Data.OnQualityChanged += TryUpdateURPAsset;
        }
        public void Dispose()
        {
            SceneLoader.OnSceneLoaded -= Awake;
            SettingsData.Data.OnGraphicsChanged -= OnGraphicsChanged;
            SettingsData.Data.OnQualityChanged -= TryUpdateURPAsset;
        }
        private void Awake()
        {
            OnGraphicsChanged(SettingsData.Data.GraphicsSettings);
            TryUpdateURPAsset(SettingsData.Data.QualitySettings);
        }
        private void OnGraphicsChanged(GraphicsSettings value)
        {
            Screen.SetResolution(value.Resolution.width, value.Resolution.height, value.ScreenMode);
            Application.targetFrameRate = value.RefreshRate;
            UnityEngine.QualitySettings.vSyncCount = value.Vsync ? 1 : 0;
        }
        private void TryUpdateURPAsset(QualitySettings context)
        {
            if (!context.IsCustomAsset) return;
            UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
            UpdateURPAsset(context, urp);
        }
        private void UpdateURPAsset(QualitySettings context, UniversalRenderPipelineAsset urp)
        {
            urp.msaaSampleCount = context.MSAA;
            urp.renderScale = context.RenderScale;
            urp.maxAdditionalLightsCount = context.LightsLimit;
            urp.shadowDistance = context.ShadowDisance;
            urp.shadowCascadeCount = context.ShadowCascade;
        }
        #endregion methods
    }
}