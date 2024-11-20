using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    public interface ISingleShopItem
    {
        #region fields & properties
        public bool IsOwned { get; }
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Set <see cref="IsOwned"/> to true and do something
        /// </summary>
        public void MakeOwned();
        #endregion methods
    }
}