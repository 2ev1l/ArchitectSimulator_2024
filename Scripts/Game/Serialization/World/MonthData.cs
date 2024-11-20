using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;
using Universal.Serialization;

namespace Game.Serialization.World
{
    [System.Serializable]
    public partial class MonthData
    {
        #region fields & properties
        /// <summary>
        /// Limit after which game may be ended
        /// </summary>
        public const int MONTH_LIMIT = 720;
        public static readonly int BaseFreeTime = 168;
        /// <summary>
        /// <see cref="{T0}"/> - currentMonth
        /// </summary>
        public UnityAction<int> OnMonthChanged;
        public UnityAction OnBeforeMonthChanged;
        public int CurrentMonth => currentMonth;
        [SerializeField] private int currentMonth = 1;

        public StatisticData GainStatistic => gainStatistic;
        [SerializeField] private StatisticData gainStatistic;
        /// <summary>
        /// Statistic for month start. <br></br>
        /// In <see cref="IMonthUpdatable"/> this statistic is for old month. <br></br>
        /// Statistic updates before invoke <see cref="OnMonthChanged"/>
        /// </summary>
        public StatisticData StartStatistic => startStatistic;
        [SerializeField] private StatisticData startStatistic = new(BaseFreeTime);
        public RangedValue FreeTime => freeTime;
        [SerializeField] private RangedValue freeTime = new(BaseFreeTime, new(0, BaseFreeTime));
        #endregion fields & properties

        #region methods
        public void StartNextMonth()
        {
            OnBeforeMonthChanged?.Invoke();
            currentMonth++;
            UpdateGainStatistic();
            UpdateMonthUpdatables();
            ResetFreeTime();
            UpdateGainStatistic();
            startStatistic = new(GameData.Data.PlayerData);
            OnMonthChanged?.Invoke(currentMonth);
        }
        private void UpdateMonthUpdatables()
        {
            IReadOnlyList<IMonthUpdatable> list = GameData.Data.GetMonthUpdatables();
            foreach (var el in list)
            {
                el.OnMonthUpdate(this);
            }
        }
        private void ResetFreeTime()
        {
            MoodData mood = GameData.Data.PlayerData.Mood;
            float pow = 0.5f;
            float pow100 = 10f;
            float powValue = Mathf.Pow(mood.Value, pow);
            float timeValuePercent = Mathf.Clamp(powValue / pow100, 0, 1);
            int finalTimeValue = (int)(freeTime.Range.y * timeValuePercent);
            freeTime.SetValue(finalTimeValue);
        }
        private void UpdateGainStatistic()
        {
            gainStatistic = new(GameData.Data.PlayerData);
            gainStatistic = gainStatistic.Gain(startStatistic);
        }

        #endregion methods
    }
}