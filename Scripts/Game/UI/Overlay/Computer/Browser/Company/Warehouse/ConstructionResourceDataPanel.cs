using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ConstructionResourceDataPanel : ResourceDataPanelRequestExecutor<ConstructionResourceData>
    {
        #region fields & properties
        private GameObject ItemObject
        {
            get
            {
                if (itemObject == null)
                    itemObject = itemRect.gameObject;
                return itemObject;
            }
        }
        [System.NonSerialized] private GameObject itemObject;
        [SerializeField] private RectTransform parentRect;
        [SerializeField] private RectTransform itemRect;
        [SerializeField] private ConstructionResourceWarehouseItem dataItem;
        #endregion fields & properties

        #region methods
        protected override void OpenPanel(ResourceDataPanelRequest<ConstructionResourceData> request)
        {
            ItemObject.SetActive(true);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, CanvasInitializer.OverlayCamera, out Vector2 localPoint);
            ItemObject.transform.localPosition = new Vector3(localPoint.x + (itemRect.rect.width / 2), localPoint.y + (itemRect.rect.height / 2));
            dataItem.OnListUpdate(request.Data);
        }
        protected override void ClosePanel()
        {
            ItemObject.SetActive(false);
        }
        #endregion methods
    }
}