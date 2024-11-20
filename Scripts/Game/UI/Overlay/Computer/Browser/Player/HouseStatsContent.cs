using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Player
{
    public class HouseStatsContent : StatsContent
    {
        #region fields & properties
        [SerializeField] private HouseItem houseItem;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI rentPriceText;
        [SerializeField] private MinimalRatingExposer ratingExposer;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerData.HouseData.OnInfoChanged += UpdateUI;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerData.HouseData.OnInfoChanged -= UpdateUI;
        }
        public override void UpdateUI()
        {
            houseItem.OnListUpdate(PlayerData.HouseData.Info);
            RentableRealEstate rentableRealEstate = PlayerData.HouseData.RentableInfo;
            priceText.text = $"${rentableRealEstate.Price}";
            rentPriceText.text = $"${rentableRealEstate.RentPrice} / m.";
            ratingExposer.Expose(rentableRealEstate);
        }
        #endregion methods
    }
}