using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RentablePremiseShopItemData<Premise> : ChangableInfoRentableItemData<Premise, PremiseInfo> 
        where Premise : RentablePremise
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public RentablePremiseShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}