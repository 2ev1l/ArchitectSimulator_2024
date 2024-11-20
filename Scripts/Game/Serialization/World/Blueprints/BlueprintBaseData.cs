using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BlueprintBaseData
    {
        #region fields & properties
        /// <summary>
        /// This is the unique identifier for each BlueprintBaseData
        /// </summary>
        public int BaseId => baseId;
        [SerializeField] private int baseId = 0;
        public string Name => name;
        [SerializeField] private string name = "";
        public int BlueprintInfoId => blueprintInfoId;
        [SerializeField][Min(0)] private int blueprintInfoId;
        public BlueprintInfo BlueprintInfo
        {
            get
            {
                blueprintInfo ??= DB.Instance.BlueprintInfo[blueprintInfoId].Data;
                return blueprintInfo;
            }
        }
        [System.NonSerialized] private BlueprintInfo blueprintInfo = null;
        /// <summary>
        /// Simplified get for <see cref="BlueprintInfo.BuildingInfo"/>
        /// </summary>
        public BuildingInfo BuildingData => BlueprintInfo.BuildingInfo;
        public IReadOnlyList<BlueprintResourceData> BlueprintResources => blueprintResources;
        [SerializeField] private List<BlueprintResourceData> blueprintResources = new();
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Clamps negative values to 0
        /// </summary>
        /// <param name="id"></param>
        public void ChangeBaseId(int id)
        {
            this.baseId = Mathf.Max(0, id);
        }
        public virtual bool TryChangeName(string name)
        {
            if (!IsNameAllowed(name)) return false;
            this.name = name;
            return true;
        }
        public static bool IsNameAllowed(string name)
        {
            return name.Length > 0;
        }
        /// <summary>
        /// Prepare for <see cref="AddResource(BlueprintResource)"/>
        /// </summary>
        public virtual void ClearData()
        {
            blueprintResources.Clear();
        }
        /// <summary>
        /// Make sure data is clear before adding
        /// </summary>
        /// <param name="element"></param>
        public void AddResource(BlueprintResource element, BuildingFloor floorPlaced)
        {
            BlueprintResourceData bd = new(element.Transform.localPosition, element.RotationScale, floorPlaced, element.ConstructionReferenceId, element.ChoosedColor);
            blueprintResources.Add(bd);
        }
        public BlueprintBaseData(string name, int blueprintInfoId, IReadOnlyList<BlueprintResourceData> blueprintResources)
        {
            this.name = name;
            this.blueprintInfoId = Mathf.Max(blueprintInfoId, 0);
            this.blueprintResources = blueprintResources.ToList();
        }
        #endregion methods
    }
}