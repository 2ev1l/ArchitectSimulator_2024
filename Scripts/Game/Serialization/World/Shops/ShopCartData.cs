using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ShopCartData<T> : ShopData<T> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        public abstract CartData<T> Cart { get; }
        #endregion fields & properties

        #region methods

        public override void GenerateNewData()
        {
            Cart.Clear();
            base.GenerateNewData();
        }
        #endregion methods
    }
}