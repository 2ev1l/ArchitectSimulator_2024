using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class LandPlotOfferData
    {
        #region fields & properties
        private int HumanId
        {
            get
            {
                if (humanId < 0)
                    humanId = Random.Range(0, DB.Instance.HumanInfo.Data.Count);
                return humanId;
            }
        }
        [SerializeField][Min(-1)] private int humanId = -1;
        public string Description => LanguageInfo.GetTextByType(TextType.Game, descriptionGameTextId);
        [SerializeField][Min(0)] private int descriptionGameTextId = 0;
        public SellingLandPlotData SellingPlot => sellingPlot;
        [SerializeField] private SellingLandPlotData sellingPlot;
        public int SellingPrice => sellingPrice;
        [SerializeField] private int sellingPrice = 0;
        public HumanInfo HumanInfo
        {
            get
            {
                humanInfo ??= DB.Instance.HumanInfo[HumanId].Data;
                return humanInfo;
            }
        }
        [System.NonSerialized] private HumanInfo humanInfo = null;
        #endregion fields & properties

        #region methods
        public LandPlotOfferData(SellingLandPlotData sellingPlot, int sellingPrice, int descriptionGameTextId)
        {
            this.sellingPlot = sellingPlot;
            this.sellingPrice = sellingPrice;
            this.descriptionGameTextId = descriptionGameTextId;
        }
        #endregion methods
    }
}