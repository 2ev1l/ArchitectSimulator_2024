using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionResourceShopItemData : ResourceShopItemData<BuyableConstructionResource>, ICloneable<ConstructionResourceShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            GameData.Data.CompanyData.WarehouseData.TryAddConstructionResource(Id, count);
        }
        protected override BuyableConstructionResource GetInfo()
        {
            int id = Id;
            return DB.Instance.BuyableConstructionResourceInfo.Find(x => x.Data.ResourceInfo.Id == id).Data;
        }

        public ConstructionResourceShopItemData Clone() => new(Id, StartPrice, Discount);
        public ConstructionResourceShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}