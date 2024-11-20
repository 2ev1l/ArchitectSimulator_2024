using Game.DataBase;
using Game.Serialization.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentablePremiseShopCartBehaviour<ShopItem, Premise> : VirtualShopCartBehaviour<ShopItem>
        where ShopItem : RentablePremiseShopItemData<Premise> , ICloneable<ShopItem>
        where Premise : RentablePremise
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}