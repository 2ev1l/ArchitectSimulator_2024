using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RealEstateShopCartItemList<ShopItem, RealEstate> : RentableObjectShopCartItemList<ShopItem, RealEstate, RealEstateInfo>
        where ShopItem : RealEstateShopItemData<RealEstate>, ICloneable<ShopItem>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override IEnumerable<VirtualShopItemContext<ShopItem>> GetFilteredItems(IEnumerable<VirtualShopItemContext<ShopItem>> currentItems)
        {
            int currentRating = GameData.Data.CompanyData.Rating.Value;
            currentItems = currentItems.Where(x => currentRating >= x.ItemData.Item.Info.MinRating);
            return base.GetFilteredItems(currentItems);
        }
        #endregion methods
    }
}