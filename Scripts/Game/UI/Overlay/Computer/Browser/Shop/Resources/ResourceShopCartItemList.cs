using DebugStuff;
using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Universal.Collections.Generic.Filters;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceShopCartItemList<ShopItem, Resource> : ConstantShopItemsList<ResourceShopCartItem<ShopItem, Resource>, ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties
        [SerializeField] private VirtualFilters<VirtualShopItemContext<ShopItem>, ResourceInfo> resourceFilters = new(x => x.ItemData.Item.Info.ResourceInfo);
        #endregion fields & properties

        #region methods
        protected override IEnumerable<VirtualShopItemContext<ShopItem>> GetFilteredItems(IEnumerable<VirtualShopItemContext<ShopItem>> currentItems)
        {
            int currentRating = GameData.Data.CompanyData.Rating.Value;
            currentItems = currentItems.Where(x => currentRating >= x.ItemData.Item.Info.MinRating);
            currentItems = resourceFilters.ApplyFilters(currentItems);
            return base.GetFilteredItems(currentItems);
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            resourceFilters.Validate(this);
        }
        #endregion methods

    }
}