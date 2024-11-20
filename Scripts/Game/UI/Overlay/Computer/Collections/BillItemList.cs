using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class BillItemList : InfinityFilteredItemListBase<BillItem, BillData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.PlayerData.BillsData.OnBillPayed += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.PlayerData.BillsData.OnBillPayed -= UpdateListData;
        }
        protected override void UpdateCurrentItems(List<BillData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<BillData> bills = GameData.Data.PlayerData.BillsData.Bills;
            int billsCount = bills.Count;
            for (int i = 0; i < billsCount; ++i)
            {
                currentItemsReference.Add(bills[i]);
            }
        }
        #endregion methods
    }
}