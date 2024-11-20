using Game.Events;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class BlueprintFloorChecker : MonoBehaviour
    {
        #region fields & properties
        public UnityEvent OnDataNull;
        public UnityEvent OnMaxFloorReached;
        public UnityEvent OnMinFloorReached;
        public UnityEvent OnOtherFloorReached;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            BlueprintEditor.Instance.OnCurrentDataChanged += Check;
            BlueprintEditor.Instance.Creator.OnFloorChanged += Check;
            Check();
        }
        private void OnDisable()
        {
            BlueprintEditor.Instance.OnCurrentDataChanged -= Check;
            BlueprintEditor.Instance.Creator.OnFloorChanged -= Check;
        }
        private void Check()
        {
            BlueprintData data = BlueprintEditor.Instance.CurrentData;
            BlueprintEditorCreator creator = BlueprintEditor.Instance.Creator;
            if (data == null)
            {
                OnDataNull?.Invoke();
                return;
            }
            if (creator.CurrentBuildingFloor == data.BuildingData.MaxFloor)
            {
                OnMaxFloorReached?.Invoke();
                return;
            }
            if (creator.CurrentBuildingFloor == DataBase.BuildingFloor.F1_Flooring)
            {
                OnMinFloorReached?.Invoke();
                return;
            }
            OnOtherFloorReached?.Invoke();
        }
        #endregion methods
    }
}