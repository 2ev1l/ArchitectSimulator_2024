using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class BuildingInfo
    {
        #region fields & properties
        public BuildingType BuildingType => buildingType;
        [SerializeField] private BuildingType buildingType = BuildingType.House;
        /// <summary>
        /// Allowed unknown style
        /// </summary>
        public BuildingStyle BuildingStyle => buildingStyle;
        [SerializeField] private BuildingStyle buildingStyle = BuildingStyle.American;
        public BuildingFloor MaxFloor => maxFloor;
        [SerializeField] private BuildingFloor maxFloor;
        public int Floor2AdditionalCount => floor2AdditionalCount;
        [SerializeField][Range(0, 20)] private int floor2AdditionalCount = 0;
        #endregion fields & properties

        #region methods
        public string ToLanguage(string textBeforeBuildingType, string textAfterBuildingType, string textBeforeBuildingStyle, string textAfterBuildingStyle, string textBeforeFloorNumbers)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(ResourceColor.Cyan.ToColorRGB());
            StringBuilder sb = new();
            sb.Append($"{textBeforeBuildingType}<color=#{colorHex}>{buildingType.ToLanguage()}</color>{textAfterBuildingType}");
            if (buildingStyle != 0)
                sb.Append($"\n{textBeforeBuildingStyle}<color=#{colorHex}>{buildingStyle.ToLanguage()}</color>{textAfterBuildingStyle}");
            sb.Append($"\n{textBeforeFloorNumbers}<color=#{colorHex}>{maxFloor.ToFloorsNumber(floor2AdditionalCount)}x</color>");
            return sb.ToString();
        }
        /// <summary>
        /// Make sure that no one is set as multiple flag
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="buildingStyle"></param>
        /// <param name="maxFloor"></param>
        /// <param name="floor2AdditionalCount"></param>
        public BuildingInfo(BuildingType buildingType, BuildingStyle buildingStyle, BuildingFloor maxFloor, int floor2AdditionalCount)
        {
            this.buildingType = buildingType;
            this.buildingStyle = buildingStyle;
            this.maxFloor = maxFloor;
            this.floor2AdditionalCount = floor2AdditionalCount;
        }
        #endregion methods
    }
}