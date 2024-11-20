using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum ConstructionLocation
    {
        Inside,
        Outside,
    }
    #endregion enum

    public static class ConstructionLocationExtension
    {
        #region methods
        public static string ToLanguage(this ConstructionLocation location) => location switch
        {
            ConstructionLocation.Inside => LanguageInfo.GetTextByType(TextType.Resource, 3),
            ConstructionLocation.Outside => LanguageInfo.GetTextByType(TextType.Resource, 4),
            _ => throw new System.NotImplementedException($"language for {location}"),
        };
        #endregion methods
    }
}