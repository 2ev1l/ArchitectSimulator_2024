using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class CompanyValueStats : TextStatsContent
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        [Todo("Add more checks to totalSum")]
        public override void UpdateUI()
        {
            int totalSum = PlayerData.Wallet.Value;
            CompanyData companyData = GameData.Data.CompanyData;
            IReadOnlyList<ConstructionTaskData> companyCompletedTasks = companyData.ConstructionTasks.CompletedTasks;
            foreach (var task in companyCompletedTasks)
            {
                foreach (var reward in task.Info.RewardInfo.Rewards)
                {
                    if (reward.Type != RewardType.Money) continue;
                    totalSum += reward.Value;
                }
            }
            if (companyData.WarehouseData.RentableInfo != null)
                totalSum += companyData.WarehouseData.RentableInfo.Price;
            if (companyData.OfficeData.RentableInfo != null)
                totalSum += companyData.OfficeData.RentableInfo.Price;

            Text.text = $"${totalSum}";
        }
        #endregion methods
    }
}