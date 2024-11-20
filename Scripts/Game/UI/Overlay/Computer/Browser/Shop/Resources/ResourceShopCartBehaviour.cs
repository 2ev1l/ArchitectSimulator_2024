using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class ResourceShopCartBehaviour<ShopItem, Resource> : VirtualShopCartBehaviour<ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}