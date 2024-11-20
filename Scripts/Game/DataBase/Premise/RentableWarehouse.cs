using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class RentableWarehouse : RentablePremise, IShopVisible
    {
        #region fields & properties
        public override DBScriptableObjectBase ObjectReference => info;
        public override PremiseInfo PremiseInfo => info.Data;
        [SerializeField] private WarehouseInfoSO info;
        public bool VisibleInShop => visibleInShop;
        [SerializeField] private bool visibleInShop = true;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}