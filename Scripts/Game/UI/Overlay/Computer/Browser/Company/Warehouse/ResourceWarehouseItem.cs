using EditorCustom.Attributes;
using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    [System.Serializable]
    public abstract class ResourceWarehouseItem<T> : ContextActionsItem<T> where T : ResourceData
    {
        #region fields & properties
        public WarehouseData Warehouse => GameData.Data.CompanyData.WarehouseData;

        [SerializeField] private TextMeshProUGUI volumeText;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI totalVolumeText;

        [SerializeField] private TMP_InputField inputFieldCount;

        [SerializeField] private ResourceItem resourceItem;
        private static readonly LanguageInfo removeInfo = new(73, TextType.Game);
        private static readonly LanguageInfo removeConfirmInfo = new(84, TextType.Game);
        //Optimization
        [System.NonSerialized] private int lastUpdatedId = -1;
        [System.NonSerialized] private int lastUpdatedCount = -1;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            bool idUpdated = lastUpdatedId != Context.Id;
            lastUpdatedId = Context.Id;
            if (idUpdated)
            {
                if (volumeText != null)
                    volumeText.text = GetSingleVolumeText();
            }
            if (idUpdated || lastUpdatedCount != Context.Count)
            {
                if (quantityText != null)
                    quantityText.text = GetQuantityText();
                if (totalVolumeText != null)
                    totalVolumeText.text = GetTotalVolumeText();
            }
            lastUpdatedCount = Context.Count;
        }
        public string GetSingleVolumeText()
        {
            return $"{Context.Info.Prefab.VolumeM3:F2} m3";
        }
        public string GetQuantityText()
        {
            return $"{Context.Count}x";
        }
        public string GetTotalVolumeText()
        {
            return $"{(Context.Info.Prefab.VolumeM3 * Context.Count):F2} m3";
        }
        [SerializedMethod]
        public void ShowDetailsPanel()
        {
            new ResourceDataPanelRequest<T>(Context, true).Send();
        }
        [SerializedMethod]
        public void HideDetailsPanel()
        {
            new ResourceDataPanelRequest<T>(Context, false).Send();
        }
        private bool TryReadInputField(out int count)
        {
            count = System.Convert.ToInt32(inputFieldCount.text);
            if (count <= 0) return false;
            return true;
        }
        [SerializedMethod]
        public void OnRemoveButtonClicked()
        {
            if (!TryReadInputField(out int count)) return;
            ConfirmRequest confirmRemoveRequest = new(OnRemoveConfirmed, null, $"{removeInfo.Text}", $"{removeConfirmInfo.Text}\n\n{this.resourceItem.NameText} x{Mathf.Clamp(count, 0, Context.Count)}");
            confirmRemoveRequest.Send();
        }
        private void OnRemoveConfirmed()
        {
            if (!TryReadInputField(out int count)) return;
            RemoveResourceFromWarehouse(Context, count);
            UpdateUI();
        }
        protected abstract void RemoveResourceFromWarehouse(T resource, int count);
        public override void OnListUpdate(T param)
        {
            base.OnListUpdate(param);
            resourceItem.OnListUpdate(Context.Info);
        }
        #endregion methods
    }
}