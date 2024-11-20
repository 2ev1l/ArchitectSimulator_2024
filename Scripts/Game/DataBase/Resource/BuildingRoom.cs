using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum BuildingRoom
    {
        Unknown,
        Living,
        Kitchen,
        Bathroom,
        Bedroom,

        ProductionHall,
        RawMaterialStorage,
        FinishedGoodsStorage,
        Recreation,
        Canteen,
        Electrical,
        Laboratory
    }
    #endregion enum

    public static class BuildingRoomTypeExtension
    {
        #region methods
        public static string ToLanguage(this BuildingRoom type) => type switch
        {
            BuildingRoom.Unknown => LanguageInfo.GetTextByType(TextType.Resource, 14),
            BuildingRoom.Living => LanguageInfo.GetTextByType(TextType.Resource, 16),
            BuildingRoom.Kitchen => LanguageInfo.GetTextByType(TextType.Resource, 17),
            BuildingRoom.Bathroom => LanguageInfo.GetTextByType(TextType.Resource, 18),
            BuildingRoom.Bedroom => LanguageInfo.GetTextByType(TextType.Resource, 19),

            BuildingRoom.ProductionHall => LanguageInfo.GetTextByType(TextType.Resource, 58),
            BuildingRoom.RawMaterialStorage => LanguageInfo.GetTextByType(TextType.Resource, 59),
            BuildingRoom.FinishedGoodsStorage => LanguageInfo.GetTextByType(TextType.Resource, 60),
            BuildingRoom.Recreation => LanguageInfo.GetTextByType(TextType.Resource, 61),
            BuildingRoom.Canteen => LanguageInfo.GetTextByType(TextType.Resource, 62),
            BuildingRoom.Electrical => LanguageInfo.GetTextByType(TextType.Resource, 63),
            BuildingRoom.Laboratory => LanguageInfo.GetTextByType(TextType.Resource, 64),
            _ => throw new System.NotImplementedException($"language for {type}"),
        };

        private static readonly List<BuildingRoom> AllowedMultipleLivingRooms = new() { BuildingRoom.Kitchen, BuildingRoom.Bedroom };
        private static readonly List<BuildingRoom> AllowedMultipleKitchenRooms = new() { BuildingRoom.Living };
        private static readonly List<BuildingRoom> AllowedMultipleBedroomRooms = new() { BuildingRoom.Living };

        private static readonly List<BuildingRoom> AllowedMultipleRawMaterialStorageRooms = new() { BuildingRoom.FinishedGoodsStorage };
        private static readonly List<BuildingRoom> AllowedMultipleFinishedGoodsStorageRooms = new() { BuildingRoom.RawMaterialStorage };
        private static readonly List<BuildingRoom> AllowedMultipleRecreationRooms = new() { BuildingRoom.Canteen };
        private static readonly List<BuildingRoom> AllowedMultipleCanteenRooms = new() { BuildingRoom.Recreation };

        public static bool IsMultipleRoomAllowed(this BuildingRoom type, BuildingRoom multiple) => type switch
        {
            BuildingRoom.Unknown => false,
            BuildingRoom.Living => AllowedMultipleLivingRooms.Contains(multiple),
            BuildingRoom.Kitchen => AllowedMultipleKitchenRooms.Contains(multiple),
            BuildingRoom.Bathroom => false,
            BuildingRoom.Bedroom => AllowedMultipleBedroomRooms.Contains(multiple),

            BuildingRoom.ProductionHall => false,
            BuildingRoom.RawMaterialStorage => AllowedMultipleRawMaterialStorageRooms.Contains(multiple),
            BuildingRoom.FinishedGoodsStorage => AllowedMultipleFinishedGoodsStorageRooms.Contains(multiple),
            BuildingRoom.Recreation => AllowedMultipleRecreationRooms.Contains(multiple),
            BuildingRoom.Canteen => AllowedMultipleCanteenRooms.Contains(multiple),
            BuildingRoom.Electrical => false,
            BuildingRoom.Laboratory => false,

            _ => throw new System.NotImplementedException($"Room allow for {type}"),
        };
        #endregion methods
    }
}