using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentablePremiseShopCartItem<ShopItem, Premise> : ChangableInfoRentableShopCartItem<ShopItem, Premise, PremiseInfo>
        where ShopItem : RentablePremiseShopItemData<Premise>, ICloneable<ShopItem>
        where Premise : RentablePremise
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override PremiseInfo GetDBInfo(Premise rentableContext)
        {
            return rentableContext.PremiseInfo;
        }
        #endregion methods
    }
}