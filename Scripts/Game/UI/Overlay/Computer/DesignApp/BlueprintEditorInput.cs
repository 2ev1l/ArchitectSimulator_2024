using Game.Events;
using Game.Serialization.Settings;
using Game.Serialization.Settings.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Core;
using Universal.Events;
using Universal.Time;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class BlueprintEditorInput : MonoBehaviour
    {
        #region fields & properties
        private BlueprintEditor MainEditor => BlueprintEditor.Instance;
        private readonly ActionRecorder InputReverseRecorder = new(100);
        [SerializeField] private DefaultStateMachine floorInfoStateMachine;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            InputController.OnKeyDown += CheckDownKey;
            InputController.OnKeyUp += CheckUpKey;
            MainEditor.OnCurrentDataChanged += ClearRecord;
            MainEditor.Creator.OnFloorChanged += ClearRecord;
            MainEditor.Creator.OnPlacerRemoved += OnPlacerRemoved;
            MainEditor.Creator.OnPlacerAdded += OnPlacerAdded;
            MainEditor.Selector.OnSelectedElementChanged += OnSelectableChanged;
        }
        private void OnDisable()
        {
            InputController.OnKeyDown -= CheckDownKey;
            InputController.OnKeyUp -= CheckUpKey;
            MainEditor.OnCurrentDataChanged -= ClearRecord;
            MainEditor.Creator.OnFloorChanged -= ClearRecord;
            MainEditor.Creator.OnPlacerRemoved -= OnPlacerRemoved;
            MainEditor.Creator.OnPlacerAdded -= OnPlacerAdded;
            MainEditor.Selector.OnSelectedElementChanged -= OnSelectableChanged;
            ClearRecord();
        }
        private void OnSelectableChanged(BlueprintPlacerBase placer, BlueprintPlacerBase oldPlacer)
        {
            if (oldPlacer != null)
            {
                oldPlacer.OnMoveEnd -= AddMoveRecord;
            }

            if (placer != null)
            {
                placer.OnMoveEnd += AddMoveRecord;
            }
        }
        private void OnPlacerRemoved(BlueprintPlacerBase placer)
        {
            switch (placer)
            {
                case BlueprintResourcePlacer:
                    BlueprintResourcePlacer resource = placer as BlueprintResourcePlacer;
                    AddRecord(delegate { MainEditor.Creator.CurrentFloor.SpawnResource(resource.Element.ConstructionReferenceId, resource.Transform.localPosition, resource.BlueprintGraphic.RotationScale, resource.Element.ChoosedColor, false); });
                    break;
                case BlueprintRoomMarkerPlacer:
                    BlueprintRoomMarkerPlacer marker = placer as BlueprintRoomMarkerPlacer;
                    AddRecord(delegate { MainEditor.Creator.CurrentFloor.SpawnRoomMarker(marker.Marker.RoomType, marker.Transform.localPosition); });
                    break;
            }
        }
        private void OnPlacerAdded(BlueprintPlacerBase placer)
        {
            return;
            //causes loops
            /*switch (placer)
            {
                case BlueprintResourcePlacer:
                    AddRecord(delegate { MainEditor.Creator.CurrentFloor.RemoveResource(placer as BlueprintResourcePlacer); });
                    break;
                case BlueprintRoomMarkerPlacer:
                    AddRecord(delegate { MainEditor.Creator.CurrentFloor.RemoveBlueprintRoomMarker(placer as BlueprintRoomMarkerPlacer); });
                    break;
            }*/
        }
        private void ClearRecord()
        {
            InputReverseRecorder.Clear();
        }
        private void AddMoveRecord(BlueprintPlacerBase placer, Vector2 direction)
        {
            if (direction.Approximately(Vector2.zero)) return;
            AddRecord(delegate
            {
                placer.TryMoveToCoordinates(placer.Transform.localPosition * Vector2.one - direction, false);
                placer.OnMoveEnd?.Invoke(placer, Vector2.zero);
            });
        }
        private void AddRecord(System.Action action)
        {
            InputReverseRecorder.ReplaceRecord(action);
        }
        private void CheckUpKey(KeyCodeInfo keyCodeInfo)
        {
            if (BlueprintEditorValidator.FloorCheckAwait) return;
            if (floorInfoStateMachine.Context.CurrentStateId != 0) return;
            switch (keyCodeInfo.Description)
            {
                case KeyCodeDescription.DesignSelectSquare:
                    MainEditor.Selector.EndSquareSelect();
                    break;
            }
        }
        private void CheckDownKey(KeyCodeInfo keyCodeInfo)
        {
            if (BlueprintEditorValidator.FloorCheckAwait) return;
            if (floorInfoStateMachine.Context.CurrentStateId != 0) return;
            KeyCode actionKey = SettingsData.Data.KeyCodeSettings.OverlayKeys.DesignAppKeys.ActionKey.Key;
            bool isActionKeyHold = InputController.IsKeyHold(actionKey);
            switch (keyCodeInfo.Description)
            {
                case KeyCodeDescription.DesignUndo:
                    BlueprintPlacerBase lastSelected = MainEditor.Selector.SelectedElement;
                    MainEditor.Selector.DeselectAll(true);
                    InputReverseRecorder.InvokeLast();
                    MainEditor.Selector.TrySelectElement(lastSelected, true);
                    break;
                case KeyCodeDescription.DesignDeselect:
                    //dont record
                    MainEditor.Selector.DeselectAll(true);
                    break;
                case KeyCodeDescription.DesignSelectSquare:
                    MainEditor.Selector.StartSquareSelect(!isActionKeyHold);
                    break;
            }

            BlueprintPlacerBase selected = MainEditor.Selector.SelectedElement;
            CheckDownKeyForSelectedElement(keyCodeInfo, selected, isActionKeyHold);
            CheckDownKeyForSelectedElements(keyCodeInfo, isActionKeyHold);
        }

        private readonly HashSet<BlueprintPlacerBase> tempSelectedElements = new();
        private void CheckDownKeyForSelectedElements(KeyCodeInfo keyCodeInfo, bool isActionKeyHold)
        {
            MainEditor.Selector.SetSelectedElementsTo(tempSelectedElements);
            foreach (var el in tempSelectedElements)
            {
                CheckDownKeyForSelectedElement(keyCodeInfo, el, isActionKeyHold);
            }
        }
        private void CheckDownKeyForSelectedElement(KeyCodeInfo keyCodeInfo, BlueprintPlacerBase placer, bool isActionKeyHold)
        {
            if (placer == null) return;
            int moveScale = isActionKeyHold ? 5 : 1;
            switch (keyCodeInfo.Description)
            {
                case KeyCodeDescription.DesignMoveUp:
                    placer.TryMoveToCoordinates(placer.Transform.localPosition + BlueprintEditor.CELL_SIZE * moveScale * Vector3.up, true);
                    break;
                case KeyCodeDescription.DesignMoveDown:
                    placer.TryMoveToCoordinates(placer.Transform.localPosition + BlueprintEditor.CELL_SIZE * moveScale * Vector3.down, true);
                    break;
                case KeyCodeDescription.DesignMoveRight:
                    placer.TryMoveToCoordinates(placer.Transform.localPosition + BlueprintEditor.CELL_SIZE * moveScale * Vector3.right, true);
                    break;
                case KeyCodeDescription.DesignMoveLeft:
                    placer.TryMoveToCoordinates(placer.Transform.localPosition + BlueprintEditor.CELL_SIZE * moveScale * Vector3.left, true);
                    break;
                case KeyCodeDescription.DesignRotate:
                    if (isActionKeyHold)
                    {
                        placer.BlueprintGraphic.RotateTo(placer.BlueprintGraphic.RotationScale + 2);
                        AddRecord(delegate { placer.BlueprintGraphic.RotateTo(placer.BlueprintGraphic.RotationScale + 2); });
                    }
                    else
                    {
                        placer.BlueprintGraphic.Rotate();
                        AddRecord(delegate { placer.BlueprintGraphic.RotateBack(); });
                    }
                    break;
                case KeyCodeDescription.DesignRemove:
                    //dont record
                    placer.RemoveBlueprint();
                    break;
                case KeyCodeDescription.DesignDuplicate:
                    //dont record
                    placer.CloneBlueprint();
                    break;
                case KeyCodeDescription.DesignFocus:
                    MainEditor.FocusToPosition(placer.Transform.localPosition);
                    break;
            }
        }

        #endregion methods
    }
}