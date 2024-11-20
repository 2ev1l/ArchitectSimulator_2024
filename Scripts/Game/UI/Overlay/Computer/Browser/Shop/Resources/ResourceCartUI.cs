using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceCartUI<ShopItem, Resource> : CartUI<ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties
        [Title("Resource")]
        [SerializeField] private TextMeshProUGUI availableSpaceText;
        [SerializeField] private TextMeshProUGUI requiredSpaceText;
        [SerializeField] private TextMeshProUGUI shippingCostText;

        private float calculatedSpace = 0;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.CompanyData.WarehouseData.OnSpaceChanged += UpdateUI;
            GameData.Data.CompanyData.WarehouseData.OnInfoChanged += UpdateUI;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.CompanyData.WarehouseData.OnSpaceChanged -= UpdateUI;
            GameData.Data.CompanyData.WarehouseData.OnInfoChanged -= UpdateUI;
        }
        private void UpdateUI(float _1) => UpdateUI();
        protected override void UpdateUI()
        {
            base.UpdateUI();
            WarehouseData warehouse = GameData.Data.CompanyData.WarehouseData;
            availableSpaceText.text = $"{(warehouse.FreeSpace <= 0 ? $"{0:F2}" : $"{warehouse.FreeSpace:F2}")} m3";
            requiredSpaceText.text = $"{calculatedSpace:F2} m3";
            shippingCostText.text = $"${((ResourceCartData<ShopItem, Resource>)Shop.CartData).GetShippingCost()}";
            bool canAddResources = warehouse.CanAddResource(calculatedSpace);
            PurchaseButton.enabled = PurchaseButton.enabled && canAddResources;
            requiredSpaceText.color = canAddResources ? NormalColor : BadColor;
        }
        protected override void ResetItemsStats()
        {
            base.ResetItemsStats();
            calculatedSpace = 0;
        }
        protected override void OnCalculateEachItemStats(CountableItem<ShopItem> x)
        {
            base.OnCalculateEachItemStats(x);
            calculatedSpace += x.Item.Info.ResourceInfo.Prefab.VolumeM3 * x.Count;
        }
        #endregion methods
    }
}