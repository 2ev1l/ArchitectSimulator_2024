using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Serialization;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class LocationsData
    {
        #region fields & properties
        public UnityAction<int> OnLocationChanged;
        public int CurrentLocationId
        {
            get => currentLocationId;
            set
            {
                ChangeLocationID(value);
            }
        }
        [SerializeField][Min(0)] private int currentLocationId = 0;
        #endregion fields & properties

        #region methods
        private void ChangeLocationID(int id)
        {
            currentLocationId = Mathf.Max(id, 0);
            OnLocationChanged?.Invoke(currentLocationId);
        }
        #endregion methods
    }
}