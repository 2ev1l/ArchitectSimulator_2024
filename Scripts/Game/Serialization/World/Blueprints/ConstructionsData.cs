using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionsData
    {
        #region fields & properties
        public UnityAction<ConstructionData> OnConstructionBuildCanceled;
        public UnityAction<ConstructionData> OnConstructionBuilded;
        public UnityAction<ConstructionData> OnConstructionBuildStarted;
        public UnityAction<ConstructionData> OnConstructionAdded;
        public IReadOnlyList<ConstructionData> Constructions => constructions;
        [SerializeField] private List<ConstructionData> constructions = new();
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Adds new entry to data. <br></br>
        /// If entry's base reference is not unique, it will not be added
        /// </summary>
        public bool TryAdd(BlueprintData blueprintData, List<BlueprintRoomData> blueprintRooms)
        {
            int baseId = blueprintData.BaseId;
            int count = constructions.Count;
            for (int i = 0; i < count; ++i)
            {
                if (constructions[i].BaseId == baseId) return false;
            }
            ConstructionData createdData = new(baseId, blueprintData.Name, blueprintData.BlueprintInfoId, blueprintData.BlueprintResources, blueprintRooms);
            constructions.Add(createdData);
            OnConstructionAdded?.Invoke(createdData);
            return true;
        }
        /// <summary>
        /// Ignores collision and returns first found
        /// </summary>
        /// <param name="blueprintInfoId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryGet(int blueprintInfoId, out ConstructionData data)
        {
            data = null;
            if (blueprintInfoId < 0) return false;
            int constructionsCount = constructions.Count;
            for (int i = 0; i < constructionsCount; ++i)
            {
                ConstructionData cd = constructions[i];
                if (cd.BlueprintInfoId == blueprintInfoId)
                {
                    data = cd;
                    return true;
                }
            }
            return false;
        }
        public bool TryGetByBaseId(int baseId, out ConstructionData data)
        {
            int constructionsCount = constructions.Count;
            for (int i = 0; i < constructionsCount; ++i)
            {
                ConstructionData cd = constructions[i];
                if (cd.BaseId == baseId)
                {
                    data = cd;
                    return true;
                }
            }
            data = null;
            return false;
        }
        public bool TryBuildByBaseId(int baseId)
        {
            if (!TryGetByBaseId(baseId, out ConstructionData construction)) return false;
            if (!construction.TryCompleteBuild()) return false;
            OnConstructionBuilded?.Invoke(construction);
            return true;
        }
        public bool TryCancelBuild(int baseId)
        {
            if (!TryGetByBaseId(baseId, out ConstructionData construction)) return false;
            if (construction.IsBuilded) return false;
            construction.RemoveBuilders();
            OnConstructionBuildCanceled?.Invoke(construction);
            return true;
        }
        public bool TryRemove(int baseId)
        {
            if (!TryGetByBaseId(baseId, out ConstructionData construction)) return false;
            construction.RemoveBuilders();
            constructions.Remove(construction);
            return true;
        }
        public bool ContainBaseIdReference(int baseId)
        {
            int count = constructions.Count;
            for (int i = 0; i < count; ++i)
            {
                if (constructions[i].BaseId == baseId)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion methods
    }
}