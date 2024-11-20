using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceShopCartItem<ShopItem, Resource> : VirtualShopCartItem<ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}