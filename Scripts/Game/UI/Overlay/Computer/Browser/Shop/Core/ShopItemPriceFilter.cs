using Game.Serialization.World;
using Universal.Collections.Filters;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ShopItemPriceFilter : InputIntFilter, ISmartFilter<ShopItemData>
    {
        #region fields & properties
        public VirtualFilter VirtualFilter => this;
        private int price = 0;
        #endregion fields & properties

        #region methods
        public void UpdateFilterData()
        {
            price = Data;
        }
        public bool FilterItem(ShopItemData item)
        {
            return item.FinalPrice < price;
        }
        #endregion methods
    }
}