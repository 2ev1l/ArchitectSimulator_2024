using Game.DataBase;
using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class ResourceItem : DBItem<ResourceInfo>
    {
        #region fields & properties
        public string NameText => nameText.text;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image previewImage;
        [SerializeField] private CustomButton previewImageButton;
        [SerializeField][Min(0)] private float imageDelay = 1f;
        private int currentPreviewId = 0;

        [SerializeField] private ResourceColorList colorsList;
        public int CurrentColorId => colorsList.CurrentColorId;
        [SerializeField] private TextMeshProUGUI sizeText;
        protected int LastUpdatedId => lastUpdatedId;
        [System.NonSerialized] private int lastUpdatedId = -1;
        #endregion fields & properties

        #region methods
        public virtual string GetName()
        {
            return $"-{Context.Id:000}";
        }
        protected void SetPreviewSprite(int previewId)
        {
            if (previewImage == null) return;
            previewImage.sprite = Context.Prefab.MaterialsInfo[CurrentColorId].ColorPreview[previewId];
            currentPreviewId = previewId;
        }
        protected void ResetPreviewSprite() => SetPreviewSprite(0);

        protected void InvokePreviewChange()
        {
            SetPreviewSprite((currentPreviewId + 1) % Context.Prefab.MaterialsInfo[CurrentColorId].ColorPreview.Count);

            InvokePreviewChangerDelayed();
        }
        private void InvokePreviewChangerDelayed() => Invoke(nameof(InvokePreviewChange), imageDelay);
        private void ResetPreviewChange() => CancelInvoke(nameof(InvokePreviewChange));

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            if (previewImageButton != null)
            {
                previewImageButton.OnEnter += InvokePreviewChange;
                previewImageButton.OnExit += ResetPreviewChange;
            }
            if (colorsList != null)
                colorsList.OnStateChanged += ResetPreviewSprite;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            if (previewImageButton != null)
            {
                previewImageButton.OnEnter -= InvokePreviewChange;
                previewImageButton.OnExit -= ResetPreviewChange;
            }
            if (colorsList != null)
                colorsList.OnStateChanged -= ResetPreviewSprite;
        }

        protected override void UpdateUI()
        {
            bool idUpdated = lastUpdatedId != Context.Id;
            base.UpdateUI();
            if (!idUpdated) return;
            lastUpdatedId = Context.Id;
            if (colorsList != null)
                colorsList.UpdateListData(Context);
            if (sizeText != null)
                sizeText.text = $"{Context.Prefab.SizeText} [L*H*W]";
            ResetPreviewSprite();
            if (nameText != null)
                nameText.text = GetName();
        }
        public override void OnListUpdate(ResourceInfo param)
        {
            //don't invoke subscribe/unsubscribe if param is the same as context
            if (Context == param)
            {
                UpdateUI();
                return;
            }
            base.OnListUpdate(param);
        }
        #endregion methods
    }
}