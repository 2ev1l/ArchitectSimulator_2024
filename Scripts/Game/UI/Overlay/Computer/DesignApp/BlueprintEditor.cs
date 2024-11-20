using EditorCustom.Attributes;
using Game.DataBase;
using Game.Environment;
using Game.Events;
using Game.Player;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal.Behaviour;
using Universal.Collections.Generic;
using Universal.Core;
using Universal.Time;
using Zenject;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class BlueprintEditor : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// Required for optimization instead of inject
        /// </summary>
        public static BlueprintEditor Instance => instance;
        private static BlueprintEditor instance;
        /// <summary>
        /// Data may be null
        /// </summary>
        public UnityAction OnCurrentDataChanged;
        /// <summary>
        /// Precision is always quarter of cell size
        /// </summary>
        public const float VECTOR_WORKFLOW_PRECISION = 5f;

        public const int CELL_SIZE = 20;
        private static readonly float ScaleStep = 1.25f;
        public static readonly LanguageInfo WarningTextInfo = new(105, TextType.Game);
        public static readonly LanguageInfo IncorrectObjectPlacementTextInfo = new(119, TextType.Game);
        public static readonly LanguageInfo IncorrectRoomsPlacementTextInfo = new(120, TextType.Game);

        [Title("Primary")]
        [SerializeField] private RectTransform blueprintContent;
        [SerializeField] private VectorTimeChanger workflowScrollPosition;

        [SerializeField] private GameObject UILock;
        [SerializeField] private Vector2 blueprintScaleRange = new(0.35f, 4f);
        internal BlueprintEditorCreator Creator => creator;
        [SerializeField] private BlueprintEditorCreator creator = new();
        [SerializeField] private DefaultStateMachine floorResultInfoStates;
        [SerializeField] private TextMeshProUGUI floorResultInfoDefaultText;
        [SerializeField] private TextMeshProUGUI floorResultInfoErrorText;
        [SerializeField] private ConstructionPreviewPanel previewPanel;
        [SerializeField] private CanvasGroup axesCanvasGroup;
        internal BlueprintEditorSelector Selector => selector;
        [SerializeField] private BlueprintEditorSelector selector = new();
        internal BlueprintEditorRooms Rooms
        {
            get
            {
                rooms ??= new(creator);
                return rooms;
            }
        }
        private BlueprintEditorRooms rooms = null;

        /// <exception cref="System.NullReferenceException"></exception>
        public BlueprintData CurrentData => currentData;
        private BlueprintData currentData = null;
        /// <summary>
        /// Clamped to cell size, represents local workflow coordinates
        /// </summary>
        public Vector2 ViewCenter
        {
            get
            {
                return RoundToCellSize(ViewCenterUnclamped);
            }
        }
        private Vector2 ViewCenterUnclamped
        {
            get
            {
                float scale = blueprintContent.localScale.x;
                float size = blueprintContent.rect.width / 2 * scale;
                Vector2 currentOffset = blueprintContent.anchoredPosition;
                return new Vector2(-size - currentOffset.x, size - currentOffset.y) / scale;
            }
        }
        private bool isInputSystemLocked = false;
        private bool isDataUpdatedOnDisable = false;
        private bool isFirstTimeOpen = false;
        #endregion fields & properties

        #region methods
#if UNITY_EDITOR
        [SerializeField] private bool testBool = false;
        private HashSet<List<BlueprintPointInfo>> resultPoints = new(50);
        private void OnDrawGizmos()
        {
            //DrawGizmoTest1();
            if (testBool)
            {
                DrawGizmoTest2();
            }
            else
            {
                DrawGizmoTest4();
            }
        }
        private void DrawGizmoTest3()
        {
            if (!Application.isPlaying) return;
            if (Selector.SelectedElement == null) return;
            RectTransform workflow = Creator.ElementsParent;
            Rooms.UpdateAllBlueprintPoints(ConstructionLocation.Inside);
            HashSet<BlueprintPointInfo> info = Rooms.ElementsPoints;
            Color startColor = Color.red;
            Color endColor = Color.cyan;
            int infoCount = info.Count;
            int currentCount = 0;
            Gizmos.color = endColor;

            List<BlueprintPointInfo> elementsContainsSelected = info.Where(x =>
                    x.AdjacentElements.Contains(Selector.SelectedElement) && x.ConnectedCoordinates.PointLocation == ConstructionLocation.Inside).ToList();

            if (elementsContainsSelected.Count == 0) return;
            infoCount = elementsContainsSelected.Count - 1;
            foreach (BlueprintPointInfo point in Rooms.ElementsPoints)
            {
                float lerp = (float)currentCount / infoCount;
                float size = Mathf.Lerp(0.1f, 0.4f, lerp);
                Gizmos.color = Color.Lerp(startColor, endColor, lerp);

                foreach (var adj in point.AdjacentPoints)
                {
                    //Gizmos.DrawWireSphere(workflow.TransformPoint(adj.LocalWorkflowCoordinates), size);
                }
                Gizmos.color = Color.green;
                GUIStyle style = new();
                style.normal.textColor = Color.cyan;
                Vector3 worldCoord = workflow.TransformPoint(point.LocalWorkflowCoordinates);
                //Gizmos.DrawWireSphere(worldCoord, size);
                Handles.Label(worldCoord, $"{point.LocalWorkflowCoordinates}", style);
                currentCount++;
            }

            return;
        }
        private void DrawGizmoTest2()
        {
            if (!Application.isPlaying) return;
            if (Selector.SelectedElement == null) return;
            RectTransform workflow = Creator.ElementsParent;
            Rooms.UpdateAllBlueprintPoints(ConstructionLocation.Inside);
            BlueprintPointInfo choosedPoint = Rooms.FindAnyPointContainElement(Selector.SelectedElement);
            if (choosedPoint == null) return;
            Rooms.FindAllLoopsByPoints(choosedPoint, this.resultPoints, ConstructionLocation.Inside);
            if (resultPoints.Count == 0) return;
            float minPointsArea = float.MaxValue;
            int minPointsCount = int.MaxValue;
            List<BlueprintPointInfo> minPointsStack = null;
            foreach (List<BlueprintPointInfo> pointsStack in resultPoints)
            {
                int currentPointsCount = pointsStack.Count;
                //if (currentPointsCount > minPointsArea) continue;
                float currentArea = Rooms.CalculateRoomArea(pointsStack);
                if (currentArea > minPointsArea) continue;
                minPointsArea = currentArea;
                minPointsCount = currentPointsCount;
                minPointsStack = pointsStack;
            }

            int currentCount = 0;
            Gizmos.color = Color.red;
            Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.cyan, Color.black };
            float[] sizes = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, };
            Color pointsColor = colors[currentCount % colors.Length];
            float gizmoSize = sizes[currentCount % sizes.Length];
            foreach (BlueprintPointInfo point in minPointsStack)
            {
                Vector3 worldCoord = workflow.TransformPoint(point.LocalWorkflowCoordinates);
                GUIStyle style = new();
                Gizmos.color = pointsColor;
                style.normal.textColor = pointsColor;
                Handles.Label(worldCoord, $"{point.LocalWorkflowCoordinates}", style);
                Gizmos.DrawWireSphere(worldCoord, gizmoSize);

                foreach (BlueprintResourcePlacer placer in point.AdjacentElements)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(placer.transform.position, Vector3.one * 0.2f);
                }
            }
        }
        private void DrawGizmoTest4()
        {
            if (!Application.isPlaying) return;
            if (Selector.SelectedElement == null) return;
            RectTransform workflow = Creator.ElementsParent;
            Rooms.UpdateAllBlueprintPoints(ConstructionLocation.Inside);
            HashSet<BlueprintPointInfo> info = Rooms.ElementsPoints;

            Color startColor = Color.red;
            Color endColor = Color.cyan;
            int infoCount = info.Count;
            int currentCount = 0;
            Gizmos.color = endColor;

            List<BlueprintPointInfo> elementsContainsSelected = info.Where(x =>
                    x.AdjacentElements.Contains(Selector.SelectedElement) && x.ConnectedCoordinates.PointLocation == ConstructionLocation.Inside).ToList();
            if (elementsContainsSelected.Count == 0) return;
            infoCount = elementsContainsSelected.Count - 1;
            foreach (BlueprintPointInfo point in elementsContainsSelected)
            {
                Vector3 worldCoord = workflow.TransformPoint(point.LocalWorkflowCoordinates);
                float lerp = (float)currentCount / infoCount;
                float size = Mathf.Lerp(0.1f, 0.4f, lerp);
                Gizmos.color = Color.Lerp(startColor, endColor, lerp);
                foreach (var adj in point.AdjacentPoints)
                {
                    Vector3 adjWorldCoord = workflow.TransformPoint(adj.LocalWorkflowCoordinates);
                    Gizmos.DrawWireSphere(adjWorldCoord, size);
                }
                Gizmos.color = Color.green;
                GUIStyle style = new();
                style.normal.textColor = Color.cyan;
                Gizmos.DrawWireSphere(worldCoord, size);
                Handles.Label(worldCoord, $"{point.LocalWorkflowCoordinates}", style);
                currentCount++;
            }

            return;
        }
        [Button(nameof(CheckTimeForBuilding))]
        private void CheckTimeForBuilding()
        {
            BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out _, out Dictionary<ConstructionResourceInfo, int> resourcesCount);
            BlueprintEditorValidator.IsTimeForBuildSufficient(resourcesCount, out int requiredTime);
            Debug.Log($"Time = {requiredTime}");
        }

        [Button(nameof(GetPriceForAllFloors))]
        private void GetPriceForAllFloors()
        {
            BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out _, out Dictionary<ConstructionResourceInfo, int> allRequiredResources);
            int floorsPrice = 0;
            int wallsPrice = 0;
            int roofsPrice = 0;
            foreach (var el in allRequiredResources)
            {
                var info = el.Key;
                int infoId = info.Id;
                var buyableInfo = DB.Instance.BuyableConstructionResourceInfo.Find(x => x.Data.ResourceInfo.Id == infoId);
                if (buyableInfo == null) continue;
                int elPrice = buyableInfo.Data.Price * el.Value;
                if (info.ConstructionType == ConstructionType.Wall) wallsPrice += elPrice;
                if (info.ConstructionType == ConstructionType.Floor) floorsPrice += elPrice;
                if (info.ConstructionType == ConstructionType.Roof) roofsPrice += elPrice;
            }
            int finalPrice = floorsPrice + wallsPrice + roofsPrice;
            Debug.Log($"F:W:R = {finalPrice} = {floorsPrice}:{wallsPrice}:{roofsPrice}");
        }
        [Button(nameof(GetCompletionTime))]
        private void GetCompletionTime()
        {
            ConstructionData d = new(this.CurrentData.BaseId, "1", this.CurrentData.BlueprintInfoId, this.CurrentData.BlueprintResources, BlueprintEditorSerializer.SerializeRooms(Creator));
            List<BuilderData> b1 = new() { new(10) };
            List<BuilderData> b2 = new() { new(20) };
            List<BuilderData> b3 = new() { new(30) };
            List<BuilderData> b4 = new() { new(40) };
            List<BuilderData> b5 = new() { new(50) };
            List<BuilderData> b6 = new() { new(60) };
            List<BuilderData> b8 = new() { new(80) };
            List<BuilderData> b9 = new() { new(90) };
            List<BuilderData> b10 = new() { new(100) };
            int t1 = d.GetCompletionTime(b1);
            int t2 = d.GetCompletionTime(b2);
            int t3 = d.GetCompletionTime(b3);
            int t4 = d.GetCompletionTime(b4);
            int t5 = d.GetCompletionTime(b5);
            int t6 = d.GetCompletionTime(b6);
            int t8 = d.GetCompletionTime(b8);
            int t10 = d.GetCompletionTime(b10);
            Debug.Log($"80={t8}; 90={t8}; 100={t10}; \n10={t1} 20={t2}; 30={t3}; \n40={t4}; 50={t5}; 60={t6};");
        }
        [Button(nameof(GetWindowsCount))]
        private void GetWindowsCount()
        {
            int c = 0;
            foreach (var floor in Creator.Floors)
            {
                foreach (ObjectPool<BlueprintPlacerBase> pool in floor.Value.ResourcesPool.Values)
                {
                    foreach (BlueprintPlacerBase p in pool.Objects)
                    {
                        if (!p.IsUsing) continue;
                        if (p is not BlueprintResourcePlacer res) continue;
                        if (res.Element.ResourceInfo.ConstructionSubtype != ConstructionSubtype.Window) continue;
                        c++;
                    }
                }
            }
            Debug.Log($"{c} - Windows");
        }
        [Button(nameof(PurchaseInsufficientResources))]
        private void PurchaseInsufficientResources()
        {
            BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out Dictionary<ConstructionResourceInfo, int> insufficientResourcesCount, out Dictionary<ConstructionResourceInfo, int> resourcesCount);
            if (insufficientResourcesCount.Count == 0)
            {
                Debug.Log("All resources are sufficient");
                return;
            }
            float totalVolume = 0;
            int totalPrice = 0;

            foreach (var kv in insufficientResourcesCount)
            {
                int count = kv.Value;

                totalVolume += kv.Key.Prefab.VolumeM3 * count;
                totalPrice += DB.Instance.BuyableConstructionResourceInfo.Find(x => x.Data.ResourceInfo.Id == kv.Key.Id).Data.Price * count;
            }
            foreach (var kv in resourcesCount)
            {
                if (kv.Key.ConstructionType == ConstructionType.Roof) continue;

                totalVolume += kv.Key.Prefab.VolumeM3;
                totalPrice += DB.Instance.BuyableConstructionResourceInfo.Find(x => x.Data.ResourceInfo.Id == kv.Key.Id).Data.Price;
            }
            bool hasError = false;
            if (!GameData.Data.CompanyData.WarehouseData.CanAddResource(totalVolume))
            {
                Debug.Log($"Can't add resources ({totalVolume:F2} / {GameData.Data.CompanyData.WarehouseData.FreeSpace:F2} m3)");
                hasError = true;
            }
            if (!GameData.Data.PlayerData.Wallet.CanDecreaseValue(totalPrice))
            {
                Debug.Log($"Can't purchase resources (${totalPrice} / ${GameData.Data.PlayerData.Wallet.Value})");
                hasError = true;
            }
            if (hasError) return;

            foreach (var kv in insufficientResourcesCount)
            {
                int count = kv.Value;
                GameData.Data.CompanyData.WarehouseData.TryAddConstructionResource(kv.Key.Id, count);
            }
            foreach (var kv in resourcesCount)
            {
                if (kv.Key.ConstructionType == ConstructionType.Roof) continue;
                GameData.Data.CompanyData.WarehouseData.TryAddConstructionResource(kv.Key.Id, 1);
            }
            GameData.Data.PlayerData.Wallet.TryDecreaseValue(totalPrice);
        }
#endif
        public void ForceInitialize()
        {
            instance = this;
        }
        private void OnEnable()
        {
            CheckUILock();
            TryUnlockInputSystem();
            previewPanel.DisablePanel();
            selector.EndSquareSelect();

            if (isDataUpdatedOnDisable)
            {
                ReloadData();
                isDataUpdatedOnDisable = false;
            }
            CheckDataExist();
            if (!isFirstTimeOpen)
            {
                isFirstTimeOpen = true;
                FocusToPositionImmediate(Vector2.zero);
            }
        }
        private void OnDisable()
        {
            TryUnlockInputSystem();
        }
        private void CheckDataExist()
        {
            if (currentData == null) return;
            foreach (BlueprintData blueprintData in GameData.Data.BlueprintsData.Blueprints)
            {
                if (blueprintData.BlueprintInfoId != currentData.BlueprintInfoId) continue;
                return;
            }
            UnloadData();
        }
        /// <summary>
        /// Also makes UI locked/unlocked
        /// </summary>
        /// <returns>True if UI is locked</returns>
        private bool CheckUILock()
        {
            bool editorDisabled = !CanOpenEditor();
            if (editorDisabled != UILock.activeSelf)
                UILock.SetActive(editorDisabled);
            return editorDisabled;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if editor is modifyable</returns>
        public bool CanOpenEditor()
        {
            return currentData != null;
        }


        /// <summary>
        /// Changes position smoothly with cost of performance
        /// </summary>
        /// <param name="localWorkflowPosition"></param>
        public void FocusToPosition(Vector2 localWorkflowPosition)
        {
            workflowScrollPosition.SetValues(blueprintContent.anchoredPosition, GetFocusPosition(localWorkflowPosition));
            workflowScrollPosition.SetActions(x => blueprintContent.anchoredPosition = x, null, delegate { return !enabled; });
            workflowScrollPosition.Restart(0.5f);
        }
        /// <summary>
        /// Great performance, but it looks confusing for user
        /// </summary>
        /// <param name="localWorkflowPosition"></param>
        public void FocusToPositionImmediate(Vector2 localWorkflowPosition)
        {
            blueprintContent.anchoredPosition = GetFocusPosition(localWorkflowPosition);
        }
        private Vector2 GetFocusPosition(Vector2 localWorkflowPosition)
        {
            float scale = blueprintContent.localScale.x;
            float size = blueprintContent.rect.width / 2;
            return new Vector2(-size - localWorkflowPosition.x, size - localWorkflowPosition.y) * scale;
        }
        public static Vector2 RoundToCellSize(Vector2 localPoint)
        {
            return new(((int)localPoint.x).RoundTo(CELL_SIZE), ((int)localPoint.y).RoundTo(CELL_SIZE));
        }

        [SerializedMethod]
        public void RequestCreateRooms()
        {
            Selector.DeselectAll(true);
            if (!BlueprintEditorValidator.IsAllObjectsPlacedCorrectly(Creator.CurrentFloor))
            {
                InfoRequest ir = new(null, WarningTextInfo, IncorrectObjectPlacementTextInfo);
                ir.Send();
                return;
            }
            ConfirmRequest cr = new(CreateRooms, null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 113));
            cr.Send();
        }
        private void CreateRooms()
        {
            Rooms.UpdateAllBlueprintPoints(ConstructionLocation.Inside);
            Rooms.UpdateAllInsideRooms();
            creator.CurrentFloor.SpawnNewRooms(Rooms.RoomsInfo);
        }

        [SerializedMethod]
        public void GenerateTestZones()
        {
            Selector.DeselectAll(true);
            if (!BlueprintEditorValidator.IsAllObjectsPlacedCorrectly(Creator.CurrentFloor))
            {
                InfoRequest ir = new(null, WarningTextInfo, IncorrectObjectPlacementTextInfo);
                ir.Send();
                return;
            }
            Creator.CurrentFloor.GenerateZonesFor(Creator.CurrentFloor, out _);
            BlueprintEditorValidator.CheckAllObjectsPlacementSmoothly(Creator.CurrentFloor);
        }
        [SerializedMethod]
        public void ClearTestZones()
        {
            Selector.DeselectAll(true);
            Creator.CurrentFloor.RemoveTestCreatedZones();
            BlueprintEditorValidator.CheckAllObjectsPlacementSmoothly(Creator.CurrentFloor);
        }
        [SerializedMethod]
        public void PreviewBlueprint()
        {
            SaveDataInstantly();
            previewPanel.EnablePanel(currentData);
        }
        /// <summary>
        /// Checks again for changed data
        /// </summary>
        [SerializedMethod]
        public void RequestConfirmFinishBuild()
        {
            if (!BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out Dictionary<ConstructionResourceInfo, int> _, out Dictionary<ConstructionResourceInfo, int> allResources))
            {
                new InfoRequest(null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 134)).Send();
                return;
            }
            if (!BlueprintEditorValidator.IsTimeForBuildSufficient(allResources, out int requiredTime))
            {
                new InfoRequest(null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 10)).Send();
                return;
            }
            ConfirmRequest cr = new(OnConfirmedFinishBuild, null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 133));
            cr.Send();
        }
        /// <summary>
        /// Final button + request panel confirmed. <br></br>
        /// </summary>
        private void OnConfirmedFinishBuild()
        {
            SaveDataInstantly();
            if (!GameData.Data.ConstructionsData.TryAdd(currentData, BlueprintEditorSerializer.SerializeRooms(creator)))
            {
                InfoRequest.GetErrorRequest(400).Send();
            }

            BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out _, out Dictionary<ConstructionResourceInfo, int> allRequiredResources);
            BlueprintEditorValidator.IsTimeForBuildSufficient(allRequiredResources, out int requiredTime);

            foreach (var kv in allRequiredResources)
            {
                GameData.Data.CompanyData.WarehouseData.RemoveConstructionResource(kv.Key.Id, kv.Value);
            }
            GameData.Data.PlayerData.MonthData.FreeTime.TryDecreaseValue(requiredTime);
            GameData.Data.BlueprintsData.TryRemoveBlueprint(currentData.Name);

            floorResultInfoStates.ApplyDefaultState();

            UnloadData();
        }
        [SerializedMethod]
        public void TryFinishBuild()
        {
            Selector.DeselectAll(true);
            if (Creator.CurrentBuildingFloor != CurrentData.BuildingData.MaxFloor)
            {
                InfoRequest ir = new(null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 132));
                ir.Send();
                return;
            }
            TryLockInputSystem();
            floorResultInfoStates.ApplyState(1);
            BlueprintEditorValidator.CheckFloor(Creator.CurrentFloor, OnFinishBuildFloorPassed, OnCheckFloorFailed, OnCheckFloorStageChanged);
        }
        private void OnFinishBuildFloorPassed()
        {
            TryUnlockInputSystem();
            StringBuilder sb = new();
            if (!BlueprintEditorValidator.IsAllResourcesSufficient(Creator, out Dictionary<ConstructionResourceInfo, int> insufficient, out Dictionary<ConstructionResourceInfo, int> allResources))
            {
                sb.Append($"{LanguageLoader.GetTextByType(TextType.Game, 134)}\n\n");
                foreach (var kv in insufficient)
                {
                    ConstructionResourceInfo info = kv.Key;
                    int totalCount = allResources[kv.Key];
                    sb.Append($"{info.NameInfo.Text}-{info.Id:000} : {totalCount - kv.Value} / {totalCount}\n");
                }
                floorResultInfoStates.ApplyState(3);
                floorResultInfoErrorText.text = sb.ToString();
                return;
            }
            if (!BlueprintEditorValidator.IsTimeForBuildSufficient(allResources, out int requiredTime))
            {
                sb.Append($"{LanguageLoader.GetTextByType(TextType.Game, 10)} : {GameData.Data.PlayerData.MonthData.FreeTime.Value} h. / {requiredTime} h.");
                floorResultInfoStates.ApplyState(3);
                floorResultInfoErrorText.text = sb.ToString();
                return;
            }
            previewPanel.DisablePanel();
            floorResultInfoStates.ApplyState(4);
        }

        [SerializedMethod]
        public void TryIncreaseFloor()
        {
            TryLockInputSystem();
            Selector.DeselectAll(true);
            floorResultInfoStates.ApplyState(1);
            BlueprintEditorValidator.CheckFloor(Creator.CurrentFloor, OnIncreaseFloorPassed, OnCheckFloorFailed, OnCheckFloorStageChanged);
        }
        private void OnCheckFloorStageChanged(string stage)
        {
            floorResultInfoDefaultText.text = stage;
        }
        private void OnCheckFloorFailed(string result)
        {
            floorResultInfoStates.ApplyState(3);
            floorResultInfoErrorText.text = result;
            TryUnlockInputSystem();
        }
        private void OnIncreaseFloorPassed()
        {
            floorResultInfoStates.ApplyState(2);
            Invoke(nameof(IncreaseFloorInstantly), 0.5f);
        }
        [Button(nameof(IncreaseFloorInstantly))]
        private void IncreaseFloorInstantly()
        {
            TryUnlockInputSystem();
            floorResultInfoStates.ApplyDefaultState();
            Creator.TryIncreaseFloor(CurrentData.BuildingData.MaxFloor);
        }
        private void TryLockInputSystem()
        {
            if (isInputSystemLocked) return;
            isInputSystemLocked = true;
            InputController.LockInputSystem(0);
        }
        private void TryUnlockInputSystem()
        {
            if (!isInputSystemLocked) return;
            isInputSystemLocked = false;
            InputController.UnlockInputSystem(0);
        }
        [SerializedMethod]
        public void TryDecreaseFloor()
        {
            Selector.DeselectAll(true);
            Creator.TryDecreaseFloor();
        }
        [SerializedMethod]
        public void DeselectCurrentElement() => Selector.DeselectAll(true);
        [SerializedMethod]
        public void ScaleUp()
        {
            Vector2 viewCenter = ViewCenterUnclamped;
            blueprintContent.localScale = Vector3.one * Mathf.Clamp(blueprintContent.localScale.x * ScaleStep, blueprintScaleRange.x, blueprintScaleRange.y);
            FocusToPositionImmediate(viewCenter);
        }
        [SerializedMethod]
        public void ScaleDown()
        {
            Vector2 viewCenter = ViewCenterUnclamped;
            blueprintContent.localScale = Vector3.one * Mathf.Clamp(blueprintContent.localScale.x / ScaleStep, blueprintScaleRange.x, blueprintScaleRange.y);
            FocusToPositionImmediate(viewCenter);
        }
        [SerializedMethod]
        public void ResetScale()
        {
            Vector2 viewCenter = ViewCenterUnclamped;
            blueprintContent.localScale = Vector3.one;
            FocusToPositionImmediate(viewCenter);
        }

        [SerializedMethod]
        public void SwitchAxesVisibility()
        {
            float defaultAlpha = 0.3f;
            axesCanvasGroup.alpha = axesCanvasGroup.alpha > 0 ? 0 : defaultAlpha;
        }
        [SerializedMethod]
        public void ResetAxesPosition()
        {
            MoveAxesPosition(Vector3.zero);
        }
        [SerializedMethod]
        public void MoveAxesToSelectedObject()
        {
            if (Selector.SelectedElement == null) return;
            Vector3 localPos = Selector.SelectedElement.Transform.localPosition;
            MoveAxesPosition(localPos + Vector3.up * 20);
        }
        private void MoveAxesPosition(Vector3 localPosition)
        {
            Vector3 offset = new(10, -10, 0);
            axesCanvasGroup.transform.localPosition = localPosition + offset;
        }

        #region Serialization
        public void UnloadData()
        {
            currentData = null;
            ReloadData();
        }
        public bool TryLoadData(string newBlueprintName)
        {
            if (currentData != null && newBlueprintName.Equals(currentData.Name)) return false;
            if (!BlueprintEditorSerializer.TryDeSerialize(newBlueprintName, ref currentData)) return false;
            if (!gameObject.activeInHierarchy)
                isDataUpdatedOnDisable = true;
            ReloadData();
            return true;
        }
        private void ReloadData()
        {
            selector.DeselectAll(true);
            if (CheckUILock())
            {
                creator.ClearAllFloors();
                OnCurrentDataChanged?.Invoke();
                return;
            }
            creator.ReloadAllFloors(currentData);
            OnCurrentDataChanged?.Invoke();
        }
        [SerializedMethod]
        public void RequestSaveData()
        {
            selector.DeselectAll(true);
            ConfirmRequest cr = new(SaveDataInstantly, null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 104));
            cr.Send();
        }
        private void SaveDataInstantly()
        {
            BlueprintEditorSerializer.Serialize(creator, ref currentData);
        }
        [SerializedMethod]
        public void RequestRemoveBlueprintElements()
        {
            selector.DeselectAll(true);
            ConfirmRequest cr = new(delegate
            {
                creator.CurrentFloor.RemoveBlueprintRooms();
                creator.CurrentFloor.RemoveBlueprintResources();
                creator.CurrentFloor.RemoveBlueprintRoomMarkers();
                //don't remove zones
            }, null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 107));
            cr.Send();
        }
        [SerializedMethod]
        public void RequestReloadBlueprint()
        {
            selector.DeselectAll(true);
            ConfirmRequest cr = new(ReloadData, null, WarningTextInfo.Text, LanguageLoader.GetTextByType(TextType.Game, 108));
            cr.Send();
        }
        #endregion Serialization

        #endregion methods
    }
}