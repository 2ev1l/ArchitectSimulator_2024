using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class FloorRoomsInfo
    {
        #region fields & properties
        public BuildingFloor Floor => floor;
        [SerializeField] private BuildingFloor floor;
        public IReadOnlyList<BuildingRoomInfo> Rooms => rooms;
        [SerializeField] private BuildingRoomInfo[] rooms;
        #endregion fields & properties

        #region methods
        public void AddRoomsTo(List<BuildingRoom> list)
        {
            int roomsCount = rooms.Length;
            for (int i = 0; i < roomsCount; ++i)
            {
                list.Add(rooms[i].Room);
            }
        }
        public string ToLanguage()
        {
            StringBuilder sb = new();
            sb.Append($">{(floor == BuildingFloor.F2 ? $"{floor.ToLanguage()} .. {BuildingFloor.F3_Roof.ToLanguage()}" : $"{floor.ToLanguage()}")}");
            int roomsCount = rooms.Length;
            for (int i = 0; i < roomsCount; ++i)
            {
                sb.Append($"\n{rooms[i].ToLanguage()}");
            }
            return sb.ToString();
        }
        #endregion methods
    }
}