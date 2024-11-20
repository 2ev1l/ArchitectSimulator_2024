using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    [System.Flags]
    public enum BuildingType
    {
        House = 1,
        Shed = 2,
        Doghouse = 4,
        Garage = 8,
        Warehouse = 16,
        Greenhouse = 32,
        Factory = 64,
        ApartmentBuilding = 128,
        HighRise = 256,
    }
    #endregion enum

    public static class BuildingTypeExtension
    {
        #region methods
        /// <summary>
        /// Doesn't work with flags <br></br>
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string ToLanguage(this BuildingType bt) => bt switch
        {
            0 => LanguageInfo.GetTextByType(TextType.Resource, 14),
            BuildingType.House => LanguageInfo.GetTextByType(TextType.Resource, 9),
            BuildingType.Shed => LanguageInfo.GetTextByType(TextType.Resource, 25),
            BuildingType.Doghouse => LanguageInfo.GetTextByType(TextType.Resource, 11),
            BuildingType.Garage => LanguageInfo.GetTextByType(TextType.Resource, 49),
            BuildingType.Warehouse => LanguageInfo.GetTextByType(TextType.Game, 42),
            BuildingType.Greenhouse => LanguageInfo.GetTextByType(TextType.Resource, 54),
            BuildingType.Factory => LanguageInfo.GetTextByType(TextType.Resource, 56),
            BuildingType.ApartmentBuilding => LanguageInfo.GetTextByType(TextType.Resource, 82),
            BuildingType.HighRise => LanguageInfo.GetTextByType(TextType.Resource, 83),
            _ => throw new System.NotImplementedException($"language for {bt}"),
            //10 = apartments
        };
        /// <summary>
        /// Doesn't work with flags <br></br>
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Vector2 GetCameraPreviewIncrease(this BuildingType bt, BuildingStyle buildingStyle) => bt switch
        {
            BuildingType b when b == BuildingType.House && buildingStyle.HasFlag(BuildingStyle.Ancient) => new(3f, 4f),
            BuildingType.House => new(3f, 7f),
            BuildingType.Shed => new(2f, 4f),
            BuildingType.Doghouse => new(1f, 1f),
            BuildingType.Garage => new(2f, 4f),
            BuildingType.Warehouse => new(3f, 6.5f),
            BuildingType.Greenhouse => new(2f, 4f),
            BuildingType.Factory => new(3f, 5.5f),
            BuildingType.ApartmentBuilding => new(4f, 5.5f),
            BuildingType.HighRise => new(4.5f, 5.5f),
            _ => new(3f, 7f),
        };
        #endregion methods
    }
}