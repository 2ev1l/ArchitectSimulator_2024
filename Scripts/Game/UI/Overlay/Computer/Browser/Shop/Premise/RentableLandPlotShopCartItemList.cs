using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RentableLandPlotShopCartItemList : RentableObjectShopCartItemList<RentableLandPlotShopItemData, RentableLandPlot, PremiseInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override IEnumerable<VirtualShopItemContext<RentableLandPlotShopItemData>> GetFilteredItems(IEnumerable<VirtualShopItemContext<RentableLandPlotShopItemData>> currentItems)
        {
            int currentRating = GameData.Data.CompanyData.Rating.Value;
            currentItems = currentItems.Where(x => currentRating >= x.ItemData.Item.Info.MinRating);
            return base.GetFilteredItems(currentItems);
        }
        #endregion methods
    }
}