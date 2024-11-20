using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum ConstructionType
    {
        Wall,
        Floor,
        Roof,
    }
    #endregion enum

    public static class ConstructionTypeExtension
    {
        #region methods
        public static string ToLanguage(this ConstructionType constructionType) => constructionType switch
        {
            ConstructionType.Wall => LanguageInfo.GetTextByType(TextType.Resource, 0),
            ConstructionType.Floor => LanguageInfo.GetTextByType(TextType.Resource, 5),
            ConstructionType.Roof => LanguageInfo.GetTextByType(TextType.Resource, 15),
            _ => throw new System.NotImplementedException($"language for {constructionType}"),
        };
        #endregion methods
    }
}