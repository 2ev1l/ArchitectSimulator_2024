using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class VehicleShopItemData : ChangableInfoBuyableItemData<BuyableVehicle, VehicleInfo>, ICloneable<VehicleShopItemData>, ISingleShopItem
    {
        #region fields & properties
        public static readonly float ReturnValue = 0.3f;
        public bool IsOwned => isOwned;
        [SerializeField] private bool isOwned = false;
        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            int oldPrice = 0;
            if (GameData.Data.PlayerData.VehicleData.BuyableInfo != null)
                oldPrice = GameData.Data.PlayerData.VehicleData.BuyableInfo.Price;
            base.OnPurchase(count);
            MakeOwned();
            int returnPrice = Mathf.RoundToInt(oldPrice * ReturnValue);
            GameData.Data.PlayerData.Wallet.TryIncreaseValue(returnPrice);
        }
        public void MakeOwned()
        {
            isOwned = true;
        }
        protected override ChangableInfoData<VehicleInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.VehicleData;
        }

        protected override BuyableVehicle GetInfo()
        {
            int id = Id;
            return DB.Instance.BuyableVehicleInfo.Find(x => x.Id == id).Data;
        }

        public VehicleShopItemData Clone() => new(Id, StartPrice, Discount);
        public VehicleShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}