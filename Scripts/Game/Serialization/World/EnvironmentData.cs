using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class EnvironmentData : IMonthUpdatable
    {
        #region fields & properties
        public const int TOTAL_COLLECTIBLES = 24;

        public UnityAction<TransportType> OnLastTransportUsedChanged;
        public TransportType LastTransportUsed
        {
            get => lastTransportUsed;
            set => SetLastTransportUsed(value);
        }
        [SerializeField] private TransportType lastTransportUsed = TransportType.Unknown;
        public UniqueList<int> Collectibles => collectibles;
        [SerializeField] private UniqueList<int> collectibles = new();
        #endregion fields & properties

        #region methods
        public void OnMonthUpdate(MonthData currentMonth)
        {
            SetLastTransportUsed(TransportType.Unknown);
        }
        private void SetLastTransportUsed(TransportType value)
        {
            lastTransportUsed = value;
            OnLastTransportUsedChanged?.Invoke(lastTransportUsed);
        }
        #endregion methods
    }
}