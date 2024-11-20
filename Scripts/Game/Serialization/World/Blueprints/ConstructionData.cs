using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionData : BlueprintBaseData
    {
        #region fields & properties
        public UnityAction OnBuildCompleted;
        public UnityAction OnBuildersChanged;
        public IReadOnlyList<BlueprintRoomData> BlueprintRooms => blueprintRooms;
        [SerializeField] private List<BlueprintRoomData> blueprintRooms = new();
        public bool IsBuilded => isBuilded;
        [SerializeField] private bool isBuilded = false;
        public IReadOnlyList<BuilderData> Builders => BuildersConverted;
        [SerializeField] private List<int> builders = new();
        private List<BuilderData> BuildersConverted
        {
            get
            {
                buildersConverted ??= new();
                if (buildersConverted.Count != builders.Count)
                {
                    buildersConverted.Clear();
                    int buildersCount = builders.Count;
                    IReadOnlyList<IEmployee> existBuilders = GameData.Data.CompanyData.OfficeData.Divisions.Builders.Employees;
                    for (int i = 0; i < buildersCount; ++i)
                    {
                        existBuilders.Exists(x => x.Id == builders[i], out IEmployee existBuilder);
                        buildersConverted.Add((BuilderData)existBuilder);
                    }
                }
                return buildersConverted;
            }
        }
        [System.NonSerialized] private List<BuilderData> buildersConverted = new();
        public int BuildCompletionMonth => buildCompletionMonth;
        [SerializeField][Min(-1)] private int buildCompletionMonth = -1;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// You can't change the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool TryChangeName(string name) => false;
        public bool TryStartBuild()
        {
            if (!CanStartBuild()) return false;
            buildCompletionMonth = GameData.Data.PlayerData.MonthData.CurrentMonth + GetCompletionTime(Builders);
            GameData.Data.ConstructionsData.OnConstructionBuildStarted?.Invoke(this);
            return true;
        }
        public bool CanStartBuild()
        {
            if (builders.Count == 0) return false;
            return true;
        }
        /// <summary>
        /// O(n^2)
        /// </summary>
        /// <param name="builders"></param>
        /// <returns></returns>
        public int GetCompletionTime(IReadOnlyList<BuilderData> builders)
        {
            if (builders == null || builders.Count == 0) return -1;
            float totalVolume = 0;
            float complexityPower = 2f;
            float floorsScale = 1f;
            if (BuildingData.MaxFloor == BuildingFloor.F3_Roof && BuildingData.Floor2AdditionalCount > 0)
            {
                float sub = (BuildingData.Floor2AdditionalCount - 0.66f);
                floorsScale += Mathf.Min(Mathf.Pow(sub * 0.7f, 0.5f), sub);
            }
            foreach (var res in BlueprintResources)
            {
                ConstructionResourceInfo resInfo = res.ResourceInfo;
                int resId = resInfo.Id;
                float resourceComplexity = 0f;
                BuyableConstructionResourceSO buyableResource = DB.Instance.BuyableConstructionResourceInfo.Find(x => x.Data.ResourceInfo.Id == resId);
                if (buyableResource != null)
                {
                    resourceComplexity = buyableResource.Data.MinRating / 100f;
                }
                totalVolume += res.ResourceInfo.Prefab.VolumeM3 * Mathf.Clamp(Mathf.Pow(1 + resourceComplexity, complexityPower), 1, 2.5f) * floorsScale;
            }
            float builderDefaultSpeed = 15;
            float speedPerSkill = 0.35f;
            float buildersSpeed = builders.Count * builderDefaultSpeed;
            foreach (BuilderData builder in builders)
            {
                buildersSpeed += builder.SkillLevel * speedPerSkill;
            }
            int minTime = builders.Count == 0 ? 0 : 1;
            return Mathf.Max(minTime, Mathf.CeilToInt(totalVolume / buildersSpeed));
        }
        internal bool TryCompleteBuild()
        {
            if (isBuilded) return false;
            isBuilded = true;
            RemoveBuilders();
            OnBuildCompleted?.Invoke();
            return true;
        }
        public bool TryAddBuilder(BuilderData builder)
        {
            if (isBuilded) return false;
            if (builders.Exists(x => x == builder.Id)) return false;
            builders.Add(builder.Id);
            builder.SetBusy();
            OnBuildersChanged?.Invoke();
            return true;
        }
        public void RemoveBuilders()
        {
            foreach (var el in Builders)
            {
                el.SetFree();
            }
            builders.Clear();
            _ = Builders;
            if (!isBuilded)
                buildCompletionMonth = -1;
            OnBuildersChanged?.Invoke();
        }
        public void CalculateRoomDivergence(List<RoomDivergence> roomsAreaDivergence, out float roomsCountDivergence)
        {
            IReadOnlyList<FloorRoomsInfo> taskRooms = BlueprintInfo.RoomsInfo;
            IReadOnlyList<BlueprintRoomData> completedRooms = BlueprintRooms;

            int totalRequiredRooms = 0;
            int totalFoundRooms = 0;
            foreach (FloorRoomsInfo floorRooms in taskRooms)
            {
                BuildingFloor currentFloor = floorRooms.Floor;
                foreach (BuildingRoomInfo room in floorRooms.Rooms)
                {
                    totalRequiredRooms++;
                    float targetArea = room.TargetArea;
                    float targetMultipleArea = targetArea * BuildingRoomInfo.MultipleRoomsAreaScale;
                    BuildingRoom targetRoom = room.Room;
                    int roomsFound = 0;
                    float roomsMinDivergence = 1f;
                    foreach (BlueprintRoomData completedRoom in completedRooms)
                    {
                        if (completedRoom.FloorPlaced != currentFloor) continue;
                        BuildingRoom room1 = completedRoom.RoomType1;
                        BuildingRoom room2 = completedRoom.RoomType2;
                        float completedArea = completedRoom.Area;
                        //if room is multiple
                        if ((room1 == targetRoom || room2 == targetRoom) && room2 != BuildingRoom.Unknown && room1 != BuildingRoom.Unknown)
                        {
                            float diff = 1 - Mathf.Clamp01(completedArea / targetMultipleArea);
                            if (roomsMinDivergence > diff)
                                roomsMinDivergence = diff;
                            roomsFound++;
                            if (diff.Approximately(0))
                                roomsMinDivergence = 0;
                            continue;
                        }
                        //if room is single
                        if (room1 == targetRoom && room2 == BuildingRoom.Unknown)
                        {
                            float diff = 1 - Mathf.Clamp01(completedArea / targetArea);
                            if (roomsMinDivergence > diff)
                                roomsMinDivergence = diff;
                            roomsFound++;
                            if (diff.Approximately(0))
                                roomsMinDivergence = 0;
                            continue;
                        }
                    }
                    totalFoundRooms++;
                    RoomDivergence div = new(currentFloor, targetRoom, targetArea, roomsMinDivergence);

                    roomsAreaDivergence.Add(div);
                }
            }
            roomsCountDivergence = 1 - Mathf.Clamp01(totalFoundRooms / (float)totalRequiredRooms);
        }
        public ConstructionData(int baseId, string name, int blueprintInfoId, IReadOnlyList<BlueprintResourceData> blueprintResources, List<BlueprintRoomData> blueprintRooms) : base(name, blueprintInfoId, blueprintResources)
        {
            this.blueprintRooms = blueprintRooms.ToList();
            ChangeBaseId(baseId);
        }
        #endregion methods

        public struct RoomDivergence
        {
            public BuildingFloor Floor;
            public BuildingRoom Room;
            public float TargetArea;
            public float MaxOccupiedArea;
            /// <summary>
            /// 0..1; 0 = OK; 1 = BAD
            /// </summary>
            public float Divergence;

            public RoomDivergence(BuildingFloor floor, BuildingRoom room, float targetArea, float divergence)
            {
                Floor = floor;
                Room = room;
                TargetArea = targetArea;
                Divergence = Mathf.Clamp01(divergence);
                MaxOccupiedArea = targetArea * (1 - divergence);
            }
        }
    }
}