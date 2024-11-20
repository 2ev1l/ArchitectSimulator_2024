using Game.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RentablePremiseCartData<ShopItem, Premise> : ChangableInfoCartData<ShopItem, Premise, PremiseInfo>
        where ShopItem : RentablePremiseShopItemData<Premise>, ICloneable<ShopItem>
        where Premise : RentablePremise
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}