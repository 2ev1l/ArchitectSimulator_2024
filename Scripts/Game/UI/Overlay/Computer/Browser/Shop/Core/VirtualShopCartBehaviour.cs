using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class VirtualShopCartBehaviour<T> : VirtualShopBehaviour<T> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        public virtual CartData<T> CartData => ((ShopCartData<T>)Data).Cart;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}