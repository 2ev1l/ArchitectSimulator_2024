using Game.DataBase;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Collections
{
    public class ConstructionResourceItem : ResourceItem
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI constructionLocationText;
        [SerializeField] private TextMeshProUGUI buildingTypeText;
        [SerializeField] private TextMeshProUGUI buildingStyleText;
        [SerializeField] private TextMeshProUGUI buildingFloorText;
        private static readonly LanguageInfo unknownInfo = new(14, TextType.Resource);
        private readonly System.Text.StringBuilder stringBuilder = new();
        #endregion fields & properties

        #region methods
        public override string GetName()
        {
            ConstructionResourceInfo info = (ConstructionResourceInfo)Context;
            string nameDefault = info.NameInfo.Text;
            if (info.ConstructionType != ConstructionType.Wall)
            {
                return $"{nameDefault}{base.GetName()}";
            }
            string nameSubtype = info.ConstructionSubtype.ToLanguage();
            if (nameDefault.Equals(nameSubtype))
            {
                return $"{nameDefault}{base.GetName()}";
            }
            else
            {
                return $"{nameDefault}-{nameSubtype}{base.GetName()}";
            }
        }
        protected override void UpdateUI()
        {
            bool idUpdated = LastUpdatedId != Context.Id;
            base.UpdateUI();

            ConstructionResourceInfo info = (ConstructionResourceInfo)Context;
            if (!idUpdated) return;

            if (constructionLocationText != null)
            {
                constructionLocationText.text = $"{info.ConstructionLocation.ToLanguage()}";
            }
            if (buildingTypeText != null)
            {
                buildingTypeText.text = GetBuildingTypeText(stringBuilder, info.BuildingType);
            }

            if (buildingStyleText != null)
            {
                buildingStyleText.text = GetBuildingStyleText(stringBuilder, info.BuildingStyle);
            }

            if (buildingFloorText != null)
            {
                buildingFloorText.text = GetBuildingFloorText(stringBuilder, info.BuildingFloor);
            }
        }
        public static string GetBuildingTypeText(StringBuilder sb, BuildingType bt)
        {
            sb.Clear();
            bt.ForEachFlag(x => sb.Append($"{x.ToLanguage()}, "));
            return TryFixUnknownText(sb.ToString(), ((BuildingType)0).ToLanguage());
        }
        public static string GetBuildingStyleText(StringBuilder sb, BuildingStyle bs)
        {
            sb.Clear();
            bs.ForEachFlag(x => sb.Append($"{x.ToLanguage()}, "));
            return TryFixUnknownText(sb.ToString(), ((BuildingStyle)0).ToLanguage());
        }
        public static string GetBuildingFloorText(StringBuilder sb, BuildingFloor bf)
        {
            sb.Clear();
            bf.ForEachFlag(x => sb.Append($"{x.ToLanguage()}, "));
            return TryFixUnknownText(sb.ToString(), unknownInfo.Text);
        }
        private static string TryFixUnknownText(string text, string unknownText)
        {
            if (text.Length > 0)
                text = text.Remove(text.Length - 2, 2);
            else
                text = unknownText;
            return text;
        }
        #endregion methods
    }
}