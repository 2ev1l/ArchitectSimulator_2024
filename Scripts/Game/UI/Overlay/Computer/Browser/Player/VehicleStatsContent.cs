using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Player
{
    public class VehicleStatsContent : StatsContent
    {
        #region fields & properties
        private VehicleData Context => GameData.Data.PlayerData.VehicleData;
        [SerializeField] private GameObject emptyUI;
        [SerializeField] private VehicleItem vehicleItem;
        [SerializeField] private TextMeshProUGUI priceText;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            Context.OnInfoChanged += UpdateUI;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Context.OnInfoChanged -= UpdateUI;
        }
        public override void UpdateUI()
        {
            bool exist = Context.Id > -1;
            if (!exist)
            {
                emptyUI.SetActive(true);
                vehicleItem.gameObject.SetActive(false);
            }
            else
            {
                emptyUI.SetActive(false);
                vehicleItem.gameObject.SetActive(true);
                vehicleItem.OnListUpdate(Context.Info);
                priceText.text = $"${Context.BuyableInfo.Price}";
            }
        }
        #endregion methods
    }
}