using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay
{
    public class DescriptionLandPlotInfoPanel : DescriptionPremiseInfoPanel<LandPlotInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI descriptionText;
        #endregion fields & properties

        #region methods
        protected override void OnUpdateUI()
        {
            base.OnUpdateUI();
            BuildingInfo buildingInfo = Data.BlueprintInfo.BuildingInfo;
            StringBuilder sb = new();
            string colorHex = ColorUtility.ToHtmlStringRGB(ResourceColor.Cyan.ToColorRGB());
            sb.Append($"{GetGameText(274)}: \n  <color=#{colorHex}>{buildingInfo.BuildingType.ToLanguage()} - {buildingInfo.BuildingStyle.ToLanguage()}</color>");
            sb.Append($"\n  {BuildingFloor.F1_Flooring.ToLanguage()} .. {buildingInfo.MaxFloor.ToLanguage()} = <color=#{colorHex}>{buildingInfo.MaxFloor.ToFloorsNumber(buildingInfo.Floor2AdditionalCount)}x</color>");

            descriptionText.text = sb.ToString();
        }

        private static string GetGameText(int id) => LanguageLoader.GetTextByType(TextType.Game, id);
        #endregion methods
    }
}