using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment
{
    public class DynamicHousePrefab : StaticHousePrefab
    {
        #region fields & properties
        public Transform SafePlayerPosition => safePlayerPosition;
        [SerializeField] private Transform safePlayerPosition;
        public GameObject Laptop => laptop;
        [SerializeField] private GameObject laptop;
        public Bed Bed => bed;
        [SerializeField] private Bed bed;
        public HoursBuyableObject Shoes => shoes;
        [SerializeField] private HoursBuyableObject shoes;
        public VehicleLoader VehicleLoader => vehicleLoader;
        [SerializeField] private VehicleLoader vehicleLoader;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}