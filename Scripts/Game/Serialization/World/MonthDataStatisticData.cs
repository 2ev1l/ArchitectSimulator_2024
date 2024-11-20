using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Serialization.World
{
    public partial class MonthData
    {
        [System.Serializable]
        public struct StatisticData
        {
            #region fields & properties
            public int Money;
            public int CompletedTasks;
            public int Time;
            public int Mood;
            public int Saturation;
            #endregion fields & properties

            #region methods
            public readonly int GetIncreasedValuesCount()
            {
                int result = 0;
                if (Money > 0) result++;
                if (CompletedTasks > 0) result++;
                if (Time > 0) result++;
                if (Mood > 0) result++;
                if (Saturation > 0) result++;
                return result;
            }
            public StatisticData Gain(StatisticData startData)
            {
                StatisticData result = new()
                {
                    Money = Money - startData.Money,
                    CompletedTasks = CompletedTasks - startData.CompletedTasks,
                    Time = Time - startData.Time,
                    Mood = Mood - startData.Mood,
                    Saturation = Saturation - startData.Saturation,
                };
                return result;
            }
            public StatisticData(PlayerData playerData)
            {
                Money = playerData.Wallet.Value;
                CompletedTasks = playerData.Tasks.CompletedTasks.Count;
                Time = playerData.MonthData.FreeTime.Value;
                Mood = playerData.Mood.Value;
                Saturation = playerData.Food.TotalSaturation;
            }
            public StatisticData(int time)
            {
                Money = 0;
                CompletedTasks = 0;
                Mood = 100;
                Time = time;
                Saturation = 0;
            }
            #endregion methods
        }
    }
}