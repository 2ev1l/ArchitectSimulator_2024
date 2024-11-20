using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class HouseShopCartItem : RealEstateShopCartItem<HouseShopItemData, RentableHouse>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<RealEstateInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.HouseData;
        }
        #endregion methods
    }
}