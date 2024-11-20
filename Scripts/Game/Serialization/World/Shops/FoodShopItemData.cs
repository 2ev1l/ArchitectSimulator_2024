using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class FoodShopItemData : BuyableObjectItemData<BuyableFood>, ICloneable<FoodShopItemData>, ISingleShopItem
    {
        #region fields & properties
        public bool IsOwned => isOwned;
        [SerializeField] private bool isOwned = false;
        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            MakeOwned();
            PlayerData playerData = GameData.Data.PlayerData;
            playerData.Food.TryIncreaseSaturation(Info.Info.Saturation);
            int moodChange = Info.Info.MoodChange;
            if (moodChange > 0)
                playerData.Mood.TryIncreaseValue(moodChange);
            else
                playerData.Mood.TryDecreaseValue(Mathf.Abs(moodChange));
        }
        public void MakeOwned()
        {
            isOwned = true;
        }
        protected override BuyableFood GetInfo()
        {
            int id = Id;
            return DB.Instance.BuyableFoodInfo.Find(x => x.Data.Info.Id == id).Data;
        }
        public FoodShopItemData Clone() => new(Id, StartPrice, Discount);
        public FoodShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods

    }
}