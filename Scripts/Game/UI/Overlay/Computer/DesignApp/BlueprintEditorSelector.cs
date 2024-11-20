using Game.DataBase;
using Game.Events;
using Game.Serialization.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Behaviour;
using Universal.Core;

namespace Game.UI.Overlay.Computer.DesignApp
{
    [System.Serializable]
    internal class BlueprintEditorSelector
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> - New Selected;<br></br>
        /// <see cref="{T1}"/> - Selected that will be changed;
        /// </summary>
        public UnityAction<BlueprintPlacerBase, BlueprintPlacerBase> OnSelectedElementChanged;
        public BlueprintPlacerBase SelectedElement => selectedElement;
        private BlueprintPlacerBase selectedElement = null;
        [SerializeField] private RectTransform squareSelector;
        private CanvasGroup SquareSelectorCanvas
        {
            get
            {
                if (squareSelectorCanvas == null)
                    squareSelectorCanvas = squareSelector.GetComponent<CanvasGroup>();
                return squareSelectorCanvas;
            }
        }
        private CanvasGroup squareSelectorCanvas = null;
        private Vector2 mouseSelectStart = Vector2.zero;
        private Vector2 mouseSelectEnd = Vector2.zero;

        public IReadOnlyCollection<BlueprintPlacerBase> SelectedElements => selectedElements;
        private readonly HashSet<BlueprintPlacerBase> selectedElements = new();
        private readonly HashSet<BlueprintPlacerBase> tempSelectedElements = new();
        private Coroutine squareSelectCoroutine = null;

        #endregion fields & properties

        #region methods
        public void StartSquareSelect(bool deselectCurrentSelected)
        {
            if (squareSelectCoroutine != null) return;
            if (deselectCurrentSelected)
                DeselectAll(true);
            EndMoveSelectedElements();
            mouseSelectStart = GetMousePointOnCanvas();
            SquareSelectorCanvas.alpha = 1;
            squareSelectCoroutine = SingleGameInstance.Instance.StartCoroutine(UpdateSquareSelect());
        }
        private IEnumerator UpdateSquareSelect()
        {
            while (true)
            {
                mouseSelectEnd = GetMousePointOnCanvas();
                float rectWidth = mouseSelectEnd.x - mouseSelectStart.x;
                float rectHeight = mouseSelectEnd.y - mouseSelectStart.y;
                Vector2 selectorCenter = Vector2.right * (rectWidth / 2) + Vector2.up * (rectHeight / 2);
                squareSelector.sizeDelta = Vector2.right * Mathf.Abs(rectWidth) + Vector2.up * Mathf.Abs(rectHeight);
                squareSelector.localPosition = (Vector3)(selectorCenter + mouseSelectStart) + Vector3.forward;
                yield return null;
            }
        }
        private Vector2 GetMousePointOnCanvas()
        {
            RectTransform elementsParent = BlueprintEditor.Instance.Creator.ElementsParent;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(elementsParent, Input.mousePosition, CanvasInitializer.OverlayCamera, out Vector2 localPoint);
            return localPoint;
        }
        public void EndSquareSelect()
        {
            SquareSelectorCanvas.alpha = 0;
            if (squareSelectCoroutine == null) return;
            SingleGameInstance.Instance.StopCoroutine(squareSelectCoroutine);
            squareSelectCoroutine = null;
            SelectElementsInSquare();
        }

        private void SelectElementsInSquare()
        {
            EndMoveSelectedElements();
            foreach (var kv in BlueprintEditor.Instance.Creator.CurrentFloor.ResourcesPool)
            {
                var pool = kv.Value;
                foreach (var obj in pool.Objects)
                {
                    if (!obj.IsUsing) continue;
                    if (!BlueprintEditorValidator.CanPlaceEntireElementInsideParent(obj.BlueprintGraphic, squareSelector)) continue;
                    TryAddSelectedElement(obj);
                }
            }
        }

        public void DeselectAll(bool forceStopMoving = false)
        {
            DeselectCurrentElement(forceStopMoving);
            ClearSelectedElements();
            EndSquareSelect();
        }
        private void DeselectCurrentElement(bool forceStopMoving = false)
        {
            TrySelectElement(null, forceStopMoving);
            EndSquareSelect();
        }
        public bool IsMultipleSelected(BlueprintPlacerBase element)
        {
            return selectedElements.Contains(element);
        }
        public void SetSelectedElementsTo(HashSet<BlueprintPlacerBase> tempSelectedElements) => selectedElements.SetElementsTo(tempSelectedElements);
        public void ClearSelectedElements()
        {
            SetSelectedElementsTo(tempSelectedElements);
            foreach (var el in tempSelectedElements)
            {
                TryRemoveSelectedElement(el);
            }
            tempSelectedElements.Clear();
        }
        public void EndMoveSelectedElements()
        {
            foreach (var el in selectedElements)
            {
                el.EndMove();
            }
        }
        public void StartMoveSelectedElements()
        {
            foreach (var el in selectedElements)
            {
                el.TryStartMove();
            }
        }
        public bool TryRemoveSelectedElement(BlueprintPlacerBase element)
        {
            if (element == null) return false;
            if (!selectedElements.Contains(element)) return false;
            selectedElements.Remove(element);
            element.OnDeselected();
            return true;
        }
        public bool TryAddSelectedElement(BlueprintPlacerBase element)
        {
            DeselectCurrentElement(true);
            if (element == null) return false;
            if (selectedElements.Contains(element)) return false;
            selectedElements.Add(element);
            element.OnSelected();
            element.transform.SetAsLastSibling();
            return true;
        }
        public bool TrySelectElement(BlueprintPlacerBase element, bool forceStopMoving = false)
        {
            if (element == selectedElement) return false;
            ClearSelectedElements();

            if (selectedElement != null)
            {
                if (!forceStopMoving && selectedElement.IsMoving) return false;
            }

            BlueprintPlacerBase oldSelectedElement = selectedElement;
            selectedElement = element;

            if (oldSelectedElement != null)
            {
                oldSelectedElement.OnDeselected();
            }

            if (selectedElement != null)
            {
                selectedElement.transform.SetAsLastSibling();
                selectedElement.OnSelected();
            }
            OnSelectedElementChanged?.Invoke(selectedElement, oldSelectedElement);
            return true;
        }

        #endregion methods
    }
}