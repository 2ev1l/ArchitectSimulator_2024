using Game.Serialization.Settings;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Events
{
    public partial class GameOverChecker : ResultChecker
    {
        #region fields & properties
        public UnityAction<EndReason> OnEndReasonGot;
        public EndReason LastCheckEndReason => lastCheckEndReason;
        private EndReason lastCheckEndReason;
        #endregion fields & properties

        #region methods
        private bool IsGameEnded(out EndReason endReason)
        {
            PlayerData playerData = GameData.Data.PlayerData;
            MonthData month = playerData.MonthData;
            endReason = null;
            if (SettingsData.Data.GameplaySettings.DisableDeath)
            {
                return false;
            }
            if (month.CurrentMonth < 15) return false;

            if (month.CurrentMonth >= MonthData.MONTH_LIMIT)
            {
                endReason = new AgeReason();
                return true;
            }
            if (playerData.Mood.Value <= 10)
            {
                if (CustomMath.GetRandomChance(30))
                {
                    endReason = new MoodReason();
                    return true;
                }
            }
            if (playerData.BillsData.HasUnpayedBills)
            {
                if (CustomMath.GetRandomChance(30))
                {
                    endReason = new BillsReason();
                    return true;
                }
            }
            if (playerData.Food.NegativeSaturation > FoodData.NEGATIVE_SATURATION_LIMIT)
            {
                if (CustomMath.GetRandomChance(30))
                {
                    endReason = new SaturationReason();
                    return true;
                }
            }
            return false;
        }
        public override bool GetResult()
        {
            bool isEnded = IsGameEnded(out lastCheckEndReason);
            if (isEnded)
            {
                OnEndReasonGot?.Invoke(lastCheckEndReason);
            }
            return isEnded;
        }
        #endregion methods
    }
}