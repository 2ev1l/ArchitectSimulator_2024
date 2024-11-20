using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using Game.UI.Overlay.Computer.DesignApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class CompletedConstructionItem : ConstructionDataItem
    {
        #region fields & properties
        [SerializeField] private CustomButton previewButton;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            previewButton.OnClicked += RequestPreview;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            previewButton.OnClicked -= RequestPreview;
        }
        private void RequestPreview()
        {
            new ConstructionPreviewRequest(Context).Send();
        }
        #endregion methods
    }
}