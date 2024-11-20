using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.DataBase;
using UnityEngine.Events;
using System.Linq;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BlueprintData : BlueprintBaseData
    {
        #region fields & properties
        public IReadOnlyList<BlueprintRoomMarkerData> BlueprintRoomMarkers => blueprintRooms;
        [SerializeField] private List<BlueprintRoomMarkerData> blueprintRooms = new();
        #endregion fields & properties

        #region methods

        public override void ClearData()
        {
            base.ClearData();
            blueprintRooms.Clear();
        }

        /// <summary>
        /// Make sure data is clear before adding
        /// </summary>
        public void AddRoomMarker(BlueprintRoomMarker roomMarker, BuildingFloor floorPlaced)
        {
            BlueprintRoomMarkerData rd = new(roomMarker.Transform.localPosition, 0, floorPlaced, roomMarker.RoomType);
            blueprintRooms.Add(rd);
        }
        public BlueprintData(string name, int blueprintInfoId, IReadOnlyList<BlueprintResourceData> blueprintResources) : base(name, blueprintInfoId, blueprintResources) { }
        #endregion methods
    }
}