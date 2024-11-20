using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class ConstructionTaskInfo : TaskInfo, IMinimalRatingHandler, IBlueprintHandler, IMaximalRatingHandler
    {
        #region fields & properties
        public int MinRating => minRating;
        [SerializeField][Range(0, 100)] private int minRating;
        /// <summary>
        /// When max rating is overreached, rating reward will not be added
        /// </summary>
        public int MaxRating => maxRating;
        [SerializeField][Range(1, 100)] private int maxRating = 1;
        public BlueprintInfo BlueprintInfo => blueprintReference.Data;
        [SerializeField] private BlueprintInfoSO blueprintReference;

        public int WindowsRequired => windowsRequired;
        [Title("Additional")][SerializeField][Min(0)] private int windowsRequired = 0;
        /// <summary>
        /// Not multiple
        /// </summary>
        public ConstructionResourceGroupType ResourcesType => resourcesType;
        [SerializeField] private ConstructionResourceGroupType resourcesType = 0;
        public bool UseSingleResourcesGroup => useSingleResourcesGroup;
        [SerializeField] private bool useSingleResourcesGroup = false;
        public bool IsRepeatable => isRepeatable;
        [SerializeField] private bool isRepeatable = true;
        public bool ForceHumanId => forceHumanId;
        [SerializeField] private bool forceHumanId = false;

        public bool CanClarifyDetails => clarificationPrice > 0;
        public int ClarificationPrice => clarificationPrice;
        [SerializeField][Min(0)] private int clarificationPrice = 0;
        public int HumanId => humanId;
        [SerializeField][DrawIf(nameof(forceHumanId), true)][Min(0)] private int humanId = 0;
        #endregion fields & properties

        #region methods
        public string GetClarificationText()
        {
            StringBuilder sb = new();
            sb.Append($"{LanguageInfo.GetTextByType(TextType.Game, 410)}: {WindowsRequired}x\n"); //windows
            List<string> featuresText = new(1);
            if (resourcesType != 0)
                featuresText.Add($"{ResourcesType.ToLanguage()}");
            if (useSingleResourcesGroup)
                featuresText.Add($"{LanguageInfo.GetTextByType(TextType.Game, 411)}");

            int featuresCount = featuresText.Count;
            sb.Append($"{LanguageInfo.GetTextByType(TextType.Game, 396)}: "); //features
            if (featuresCount > 0)
            {
                for (int i = 0; i < featuresCount; ++i)
                {
                    if (i == featuresCount - 1)
                        sb.Append($"{featuresText[i]}");
                    else
                        sb.Append($"{featuresText[i]}, ");
                }
            }
            else
            {
                sb.Append($"{LanguageInfo.GetTextByType(TextType.Game, 412)}"); //not important
            }

            return sb.ToString();
        }
        #endregion methods
    }
}