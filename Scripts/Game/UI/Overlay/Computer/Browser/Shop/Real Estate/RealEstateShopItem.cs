using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Browser.Shop;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RealEstateShopItem<ShopItem, RealEstate> : RentableObjectShopItem<ShopItem, RealEstate, RealEstateInfo>
        where ShopItem : RealEstateShopItemData<RealEstate>, ICloneable<ShopItem>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties
        [SerializeField] private MinimalRatingExposer ratingExposer;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            ratingExposer.Expose(Context.ItemData.Item.Info);
        }
        protected override RealEstateInfo GetDBInfo(RealEstate buyableObjectContext)
        {
            return buyableObjectContext.RealEstateInfo;
        }
        #endregion methods
    }
}