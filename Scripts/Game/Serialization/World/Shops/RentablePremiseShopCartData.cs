using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RentablePremiseShopCartData<ShopItem, Premise> : ShopCartData<ShopItem>
        where ShopItem : RentablePremiseShopItemData<Premise>, ICloneable<ShopItem>
        where Premise : RentablePremise
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}