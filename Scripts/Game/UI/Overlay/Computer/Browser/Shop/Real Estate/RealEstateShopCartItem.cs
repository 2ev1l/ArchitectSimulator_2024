using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RealEstateShopCartItem<ShopItem, RealEstate> : ChangableInfoRentableShopCartItem<ShopItem, RealEstate, RealEstateInfo>
        where ShopItem : RealEstateShopItemData<RealEstate>, ICloneable<ShopItem>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override RealEstateInfo GetDBInfo(RealEstate rentableContext)
        {
            return rentableContext.RealEstateInfo;
        }
        #endregion methods
    }
}