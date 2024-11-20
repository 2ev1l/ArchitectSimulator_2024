using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentableObjectShopCartItem<ShopItem, Rentable, Info> : DBShopCartItem<ShopItem, Rentable, Info>
        where ShopItem : RentableObjectItemData<Rentable>, ICloneable<ShopItem>
        where Rentable : RentableObject
        where Info : DBInfo, INameHandler
    {
        #region fields & properties
        private ConfirmRequest PurchaseConfirmRequest
        {
            get
            {
                purcahseConfirmRequest = new(delegate { base.Purchase(); }, null, $"{LanguageLoader.GetTextByType(TextType.Game, 71)}", $"" +
                    $"{DBInfo.NameInfo.Text}\n" +
                    $"<size=80%>{LanguageLoader.GetTextByType(TextType.Game, 68)}</size>\n\n" +
                    $"<size=85%>{LanguageLoader.GetTextByType(TextType.Game, 6)}: ${Context.ItemData.Item.FinalPrice}\n" +
                    $"{LanguageLoader.GetTextByType(TextType.Game, 78)}: ${Context.ItemData.Item.Info.RentPrice} / m.</size>");
                return purcahseConfirmRequest;
            }
        }
        private ConfirmRequest purcahseConfirmRequest = null;
        #endregion fields & properties

        #region methods
        protected override void Purchase()
        {
            PurchaseConfirmRequest.Send();
        }
        #endregion methods
    }
}