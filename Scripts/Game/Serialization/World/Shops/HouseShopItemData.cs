using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class HouseShopItemData : RealEstateShopItemData<RentableHouse>, ICloneable<HouseShopItemData>
    {
        #region fields & properties
        public static readonly float ReturnValue = 0.3f;
        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            int oldPrice = 0;
            if (GameData.Data.PlayerData.HouseData.RentableInfo != null)
                oldPrice = GameData.Data.PlayerData.HouseData.RentableInfo.Price;
            base.OnPurchase(count);
            int returnPrice = Mathf.RoundToInt(oldPrice * ReturnValue);
            GameData.Data.PlayerData.Wallet.TryIncreaseValue(returnPrice);
        }
        protected override ChangableInfoData<RealEstateInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.HouseData;
        }
        protected override RentableHouse GetInfo()
        {
            int id = Id;
            return DB.Instance.RentableHouseInfo.Find(x => x.Id == id).Data;
        }
        public HouseShopItemData Clone() => new(Id, StartPrice, Discount);
        public HouseShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}