using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;
using static Game.Serialization.World.ConstructionData;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class LandPlotData : RentablePremiseData
    {
        #region fields & properties
        internal static string BillDescriptionInternal => LanguageInfo.GetTextByType(TextType.Game, 272);
        public override string BillDescription => LanguageInfo.GetTextByType(TextType.Game, 272);
        /// <summary>
        /// -1 means no reference
        /// </summary>
        public int BlueprintBaseIdReference => blueprintBaseIdReference;
        [SerializeField][Min(-1)] private int blueprintBaseIdReference = -1;

        /// <summary>
        /// If there's no attached construction reference, than returns null. <br></br>
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        public ConstructionData ConstructionReference
        {
            get
            {
                if (blueprintBaseIdReference < 0) return null;
                if (constructionReference == null)
                {
                    if (!GameData.Data.ConstructionsData.TryGetByBaseId(blueprintBaseIdReference, out ConstructionData construction)) return null;
                    constructionReference = construction;
                }
                return constructionReference;
            }
        }
        [System.NonSerialized] private ConstructionData constructionReference = null;

        public int MaxSellPrice
        {
            get
            {
                int constructionPrice = CalculatedConstructionPrice;
                if (constructionPrice == 0) return BaseSellPrice;
                return BaseSellPrice + constructionPrice;
            }
        }
        private int CalculatedConstructionPrice
        {
            get
            {
                if (ConstructionReference == null)
                    return 0;
                if (!ConstructionReference.IsBuilded)
                    return 0;
                if (calculatedConstructionPrice == 0)
                    calculatedConstructionPrice = CalculateConstructionPrice();
                return calculatedConstructionPrice;
            }
        }
        [System.NonSerialized] private int calculatedConstructionPrice = 0;
        public int BaseSellPrice
        {
            get
            {
                if (baseSellPrice == 0)
                    baseSellPrice = CustomMath.Multiply(RentablePlotInfo.Price, 75);
                return baseSellPrice;
            }
        }
        [System.NonSerialized] private int baseSellPrice = 0;

        public RentableLandPlot RentablePlotInfo => (RentableLandPlot)RentableInfo;
        public LandPlotInfo PlotInfo => (LandPlotInfo)RentablePlotInfo.PremiseInfo;
        #endregion fields & properties

        #region methods
        internal int CalculateConstructionPrice()
        {
            LandPlotInfo plotInfo = PlotInfo;
            ConstructionData constructionReference = ConstructionReference;
            CalculateConstructionRoomsDivergence(constructionReference, plotInfo, out float roomsCountDivergenceUnclamped, out float roomsAreaDivergence);
            CalculateConstructionResourcesDivergence(constructionReference, plotInfo, out float windowsCountDivergence);

            float roomsCountIncreaseScale = 1 - roomsCountDivergenceUnclamped; //-inf..1
            float roomsAreaIncreaseScale = 1 - Clamp01Pow(roomsAreaDivergence, 1.2f);
            float windowsCountIncreaseScale = 1 - windowsCountDivergence;

            float roomsCountWeight = 1.3f;
            if (roomsCountIncreaseScale > 1)
                roomsCountWeight = 2f;

            float roomsAreaWeight = 2f;
            if (roomsAreaIncreaseScale > 0.9f)
                roomsAreaWeight = 1.5f;

            float windowsCountWeight = 0.7f;
            if (windowsCountIncreaseScale > 0.99f)
                windowsCountWeight = 0.3f;

            float totalWeight = roomsCountWeight + roomsAreaWeight + windowsCountWeight;

            float totalIncreaseScale = ((roomsCountIncreaseScale * roomsCountWeight) +
                                        (roomsAreaIncreaseScale * roomsAreaWeight) +
                                        (windowsCountIncreaseScale * windowsCountWeight)) / totalWeight;
            int targetPrice = plotInfo.TargetBuildingPrice;
            int result = Mathf.RoundToInt(targetPrice * totalIncreaseScale);
            return Mathf.Max(result, 0);
        }
        private float Clamp01Pow(float value, float pow) => Mathf.Clamp01(Mathf.Pow(1 + value, pow) - 1);

        private void CalculateConstructionResourcesDivergence(ConstructionData constructionReference, LandPlotInfo plotInfo, out float windowsCountDivergence)
        {
            int targetWindows = plotInfo.TargetWindows;
            IReadOnlyList<BlueprintResourceData> resources = constructionReference.BlueprintResources;
            int count = resources.Count;
            int designedWindowsCount = 0;
            for (int i = 0; i < count; ++i)
            {
                BlueprintResourceData resource = resources[i];
                if (resource.ResourceInfo.ConstructionSubtype == ConstructionSubtype.Window)
                    designedWindowsCount++;
            }
            if (targetWindows > 0)
                windowsCountDivergence = 1 - Mathf.Clamp01(designedWindowsCount / (float)targetWindows);
            else
                windowsCountDivergence = 0;
        }
        private void CalculateConstructionRoomsDivergence(ConstructionData constructionReference, LandPlotInfo plotInfo, out float roomsCountDivergenceUnclamped, out float roomsAreaDivergence)
        {
            List<RoomDivergence> roomsDivergence = new();
            constructionReference.CalculateRoomDivergence(roomsDivergence, out _);
            int roomsDivergenceCount = roomsDivergence.Count;
            float totalAreaDivergence = 0f;
            for (int i = roomsDivergenceCount - 1; i >= 0; --i)
            {
                RoomDivergence roomDivergence = roomsDivergence[i];
                float currentDivergence = roomDivergence.Divergence;
                if (roomDivergence.MaxOccupiedArea.Approximately(0, 0.1f) || currentDivergence > 0.99f)
                {
                    roomsDivergence.RemoveAt(i);
                    continue;
                }
                totalAreaDivergence += currentDivergence;
            }
            roomsDivergenceCount = roomsDivergence.Count;
            int targetRooms = plotInfo.TargetRooms;
            if (targetRooms > 0)
                roomsCountDivergenceUnclamped = 1 - (roomsDivergenceCount / (float)targetRooms);
            else
                roomsCountDivergenceUnclamped = 0;

            if (roomsDivergenceCount > 0)
                roomsAreaDivergence = totalAreaDivergence / roomsDivergenceCount;
            else
                roomsAreaDivergence = 1;
        }

        internal bool TryStartBlueprint(int blueprintBaseIdReference)
        {
            if (blueprintBaseIdReference < 0) return false;
            if (this.blueprintBaseIdReference > -1) return false;
            this.blueprintBaseIdReference = blueprintBaseIdReference;
            return true;
        }
        internal void ResetBlueprintReference()
        {
            this.blueprintBaseIdReference = -1;
        }
        public override bool CanReplaceInfo(int newInfoId) => false;
        protected override RentablePremise GetRentablePremiseInfo() => DB.Instance.RentableLandPlotInfo.Find(x => x.Data.PremiseInfo.Id == Id).Data;
        public LandPlotData(int id) : base(id) { }
        #endregion methods
    }
}