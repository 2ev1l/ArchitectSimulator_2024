using EditorCustom.Attributes;
using Game.Events;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment
{
    public class MoneyBuyableObject : BuyableObject
    {
        #region fields & properties
        public override PurchaseRequestSender PurchaseRequest => purchaseRequest;
        [Title("Purchase")][SerializeField] private MoneyPurchaseRequestSender purchaseRequest = new();
        public override int Price => price.GetValueWithInflation(Inflation, maxInflationScale);
        [SerializeField] private Wallet price = new(10);
        public int Inflation => inflation;
        [SerializeField][Min(0)] private int inflation = 0;
        [SerializeField][Min(1)] private float maxInflationScale = 5f;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}