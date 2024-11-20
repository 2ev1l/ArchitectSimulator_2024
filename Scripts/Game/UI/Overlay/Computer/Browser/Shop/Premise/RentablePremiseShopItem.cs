using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Behaviour;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentablePremiseShopItem<ShopItem, Premise> : RentableObjectShopItem<ShopItem, Premise, PremiseInfo>
        where ShopItem : RentableObjectItemData<Premise>, ICloneable<ShopItem>
        where Premise : RentablePremise
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
        protected override PremiseInfo GetDBInfo(Premise buyableObjectContext)
        {
            return buyableObjectContext.PremiseInfo;
        }
        #endregion methods
    }
}