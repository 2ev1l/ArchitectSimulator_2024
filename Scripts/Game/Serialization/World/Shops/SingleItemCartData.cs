using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class SingleItemCartData<ShopItem> : CartData<ShopItem> where ShopItem : ShopItemData, ICloneable<ShopItem>, ISingleShopItem
    {
        #region fields & properties
        public CountableItem<ShopItem> CartItem => base.Items.Count == 0 ? null : base.Items[0];
        #endregion fields & properties

        #region methods
        public override void Add(ShopItem shopItem, int count)
        {
            if (base.Items.Count > 0) return;
            count = Mathf.Min(count, 1);
            base.Add(shopItem, count);
        }

        #endregion methods
    }
}