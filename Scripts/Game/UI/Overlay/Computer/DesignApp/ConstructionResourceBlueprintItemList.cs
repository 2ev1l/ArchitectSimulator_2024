using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class ConstructionResourceBlueprintItemList : ConstructionResourceDataItemList
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            BlueprintEditor.Instance.OnCurrentDataChanged += UpdateListData;
            BlueprintEditor.Instance.Creator.OnFloorChanged += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            BlueprintEditor.Instance.OnCurrentDataChanged -= UpdateListData;
            BlueprintEditor.Instance.Creator.OnFloorChanged -= UpdateListData;
        }

        private IEnumerable<ConstructionResourceData> GetFilteredOrderedItems(IEnumerable<ConstructionResourceData> currentItems)
        {
            IEnumerable<ConstructionResourceData> items = base.GetFilteredItems(currentItems);
            items = items.OrderBy(x =>
            {
                var group = ((ConstructionResourceInfo)x.Info).RelatedGroup;
                return group == null ? -1 : group.Id;
            }).ThenByDescending(x =>
            {
                return (int)((ConstructionResourceInfo)x.Info).ConstructionLocation;
            }).ThenBy(x =>
            {
                return (int)((ConstructionResourceInfo)x.Info).ConstructionType;
            }).ThenBy(x =>
            {
                return (int)((ConstructionResourceInfo)x.Info).ConstructionSubtype;
            }).ThenByDescending(x =>
            {
                return ((ConstructionResourceInfo)x.Info).Prefab.VolumeM3;
            });
            return items;
        }
        protected override IEnumerable<ConstructionResourceData> GetFilteredItems(IEnumerable<ConstructionResourceData> currentItems)
        {
            BlueprintEditor editor = BlueprintEditor.Instance;
            if (!editor.CanOpenEditor())
            {
                return GetFilteredOrderedItems(currentItems);
            }
            BuildingFloor currentFloor = editor.Creator.CurrentBuildingFloor;
            BuildingStyle currentStyle = editor.CurrentData.BuildingData.BuildingStyle;
            BuildingType currentType = editor.CurrentData.BuildingData.BuildingType;
            ConstructionType constructionType = ConstructionType.Wall;
            if (currentFloor == BuildingFloor.F2_FlooringRoof && editor.CurrentData.BuildingData.MaxFloor != currentFloor)
            {
                currentItems = currentItems.Where(x => FilterNonWallItems(x, currentFloor, currentStyle, currentType));
                return GetFilteredOrderedItems(currentItems);
            }
            if (currentFloor == BuildingFloor.F1_Flooring)
            {
                constructionType = ConstructionType.Floor;
            }
            if (editor.CurrentData.BuildingData.MaxFloor == currentFloor)
            {
                constructionType = ConstructionType.Roof;
            }
            currentItems = currentItems.Where(x => FilterItems(x, currentFloor, currentStyle, currentType, constructionType));
            return GetFilteredOrderedItems(currentItems);
        }
        private bool FilterItems(ConstructionResourceData x, BuildingFloor currentFloor, BuildingStyle currentStyle, BuildingType currentType, ConstructionType constructionType)
        {
            ConstructionResourceInfo info = ((ConstructionResourceInfo)x.Info);
            return (info.BuildingStyle.HasFlag(currentStyle) || info.BuildingStyle == 0 || currentStyle == 0) &&
                   info.BuildingType.HasFlag(currentType) &&
                   info.BuildingFloor.HasFlag(currentFloor) &&
                   info.ConstructionType == constructionType;
        }
        private bool FilterNonWallItems(ConstructionResourceData x, BuildingFloor currentFloor, BuildingStyle currentStyle, BuildingType currentType)
        {
            ConstructionResourceInfo info = ((ConstructionResourceInfo)x.Info);
            return (info.BuildingStyle.HasFlag(currentStyle) || info.BuildingStyle == 0 || currentStyle == 0) &&
                   info.BuildingType.HasFlag(currentType) &&
                   info.BuildingFloor.HasFlag(currentFloor) &&
                   info.ConstructionType != ConstructionType.Wall;
        }

        protected override void UpdateCurrentItems(List<ConstructionResourceData> currentItemsReference)
        {
            currentItemsReference.Clear();
            if (!BlueprintEditor.Instance.CanOpenEditor())
            {
                return;
            }
            base.UpdateCurrentItems(currentItemsReference);
        }
        #endregion methods

    }
}