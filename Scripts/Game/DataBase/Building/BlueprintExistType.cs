using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum BlueprintExistType
    {
        Plot,
        Task
    }
    #endregion enum

    public static class BlueprintExistTypeExtension
    {
        #region methods
        [Todo("Text for plots")]
        public static string ToLanguage(this BlueprintExistType t, int refId) => t switch
        {
            BlueprintExistType.Plot => $"{LanguageInfo.GetTextByType(TextType.None, 0)}-{refId}",
            BlueprintExistType.Task => $"{DB.Instance.PlayerTaskInfo[refId].Data.NameInfo.Text}-{refId}",
            _ => throw new System.NotImplementedException("Language for " + t.ToString()),
        };
        #endregion methods
    }
}