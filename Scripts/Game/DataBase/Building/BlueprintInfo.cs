using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Universal.Core;

namespace Game.DataBase
{
    [System.Serializable]
    public class BlueprintInfo : DBInfo
    {
        #region fields & properties
        public BuildingInfo BuildingInfo => buildingInfo;
        [SerializeField] private BuildingInfo buildingInfo = null;

        public IReadOnlyList<FloorRoomsInfo> RoomsInfo => roomsInfo;
        [SerializeField] private List<FloorRoomsInfo> roomsInfo;
        public IReadOnlyList<BlueprintZoneInfo> BlueprintZones => blueprintZones;
        [SerializeField] private List<BlueprintZoneInfo> blueprintZones = new();
        public float DifficultyScale => difficultyScale;
        [SerializeField][Min(0.1f)] private float difficultyScale = 1f;
        #endregion fields & properties

        #region methods
        public List<BuildingRoom> GetAllowedRooms(BuildingFloor floor)
        {
            int roomsCount = roomsInfo.Count;
            List<BuildingRoom> result = new();
            if (roomsInfo.Exists(x => x.Floor == floor, out FloorRoomsInfo roomInfo))
            {
                roomInfo.AddRoomsTo(result);
            }
            return result;
        }
        public List<BuildingRoom> GetAllRooms()
        {
            int roomsCount = roomsInfo.Count;
            List<BuildingRoom> result = new();
            for (int i = 0; i < roomsCount; ++i)
            {
                roomsInfo[i].AddRoomsTo(result);
            }
            return result;
        }
        public string ToLanguage(string textBeforeBuildingType, string textAfterBuildingType, string textBeforeBuildingStyle, string textAfterBuildingStyle, string textBeforeFloorNumbers, string textBeforeRooms, string textForZeroRooms)
        {
            StringBuilder sb = new();
            sb.Append($"{buildingInfo.ToLanguage(textBeforeBuildingType, textAfterBuildingType, textBeforeBuildingStyle, textAfterBuildingStyle, textBeforeFloorNumbers)}");
            int roomsCount = roomsInfo.Count;
            if (roomsCount > 0)
            {
                sb.Append($"\n{textBeforeRooms}");
                for (int i = 0; i < roomsCount; ++i)
                {
                    sb.Append($"\n{roomsInfo[i].ToLanguage()}");
                }
            }
            else
            {
                sb.Append($"\n{textForZeroRooms}");
            }
            return sb.ToString();
        }
        #endregion methods
    }
}