using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Collections.Generic
{
    [System.Serializable]
    public class SingleItemList<IListUpdater, UpdateValue>
        where IListUpdater : Component, IListUpdater<UpdateValue>
    {
        #region fields & properties
        [SerializeField] private IListUpdater instantiatedItem;
        private UpdateValue data;
        #endregion fields & properties

        #region methods
        public void UpdateListData(UpdateValue newData)
        {
            data = newData;
            UpdateUI();
        }
        private void UpdateUI()
        {
            GameObject obj = instantiatedItem.gameObject;
            bool itemActive = data != null;
            if (obj.activeSelf != itemActive)
                obj.SetActive(itemActive);
            if (!itemActive) return;
            instantiatedItem.OnListUpdate(data);
        }
        #endregion methods
    }
}