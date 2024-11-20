using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Browser;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentablePremiseShopCartItemList<ShopItem, Premise> : RentableObjectShopCartItemList<ShopItem, Premise, PremiseInfo>
        where ShopItem : RentablePremiseShopItemData<Premise>, ICloneable<ShopItem>
        where Premise : RentablePremise
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