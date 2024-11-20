using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay
{
    public class DescriptionConstructionTaskPanel : DescriptionTaskPanel<ConstructionTaskData, ConstructionTaskInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI humanNameText;
        [SerializeField] private Image faceImage;

        #endregion fields & properties

        #region methods
        protected override void OnUpdateUI()
        {
            base.OnUpdateUI();
            HumanInfo humanInfo = Data.HumanInfo;
            if (humanInfo != null)
            {
                humanNameText.text = humanInfo.Name;
                faceImage.sprite = humanInfo.PreviewSprite;
            }
            bool descriptionNull = false;
            if (!DescriptionGroup.activeSelf)
            {
                descriptionNull = true;
                DescriptionText.text = "";
                DescriptionGroup.SetActive(true);
            }
            DescriptionText.text += $"{(descriptionNull ? "" : "\n\n")}{Data.Info.BlueprintInfo.ToLanguage($"{GetGameText(146)} ", $".", $"", $". {GetGameText(147)}", $"{GetGameText(148)}: ", $"{GetGameText(149)}: ", $"{GetGameText(167)}")}";
            if (Data.IsClarified)
            {
                DescriptionText.text += $"\n\n{Data.Info.GetClarificationText()}";
            }
        }
        private static string GetGameText(int id) => LanguageLoader.GetTextByType(TextType.Game, id);
        #endregion methods
    }
}