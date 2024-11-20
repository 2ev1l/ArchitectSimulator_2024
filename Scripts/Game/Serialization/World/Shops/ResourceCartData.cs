using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ResourceCartData<ShopItem, Resource> : CartData<ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            float resourcesVolume = 0;
            foreach (var el in Items)
            {
                resourcesVolume += el.Item.Info.ResourceInfo.Prefab.VolumeM3 * el.Count;
            }
            if (!GameData.Data.CompanyData.WarehouseData.CanAddResource(resourcesVolume)) return false;
            return base.CanPurchaseCart();
        }
        public int GetShippingCost()
        {
            float volume = 0;
            foreach (var item in Items)
            {
                volume += item.Item.Info.ResourceInfo.Prefab.VolumeM3 * item.Count;
            }
            float pricePerM3 = 1.4f;
            int cost = Mathf.RoundToInt(volume * pricePerM3);
            return cost;
        }
        public override int GetCartSum()
        {
            int sum = base.GetCartSum();
            sum += GetShippingCost();
            return sum;
        }
        #endregion methods
    }
}