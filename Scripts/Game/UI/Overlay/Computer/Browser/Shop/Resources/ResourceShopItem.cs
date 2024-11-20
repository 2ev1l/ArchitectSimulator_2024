using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceShopItem<ShopItem, Resource> : DBShopItem<ShopItem, Resource, ResourceInfo>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties
        [Title("Resource")]
        [SerializeField] private MinimalRatingExposer ratingExposer;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            ratingExposer.Expose(Context.ItemData.Item.Info);
        }
        protected override ResourceInfo GetDBInfo(Resource buyableObjectContext)
        {
            return buyableObjectContext.ResourceInfo;
        }
        #endregion methods
    }
}