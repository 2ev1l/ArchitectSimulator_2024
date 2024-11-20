using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Collections.Generic
{
    public abstract class SingleItemListBase<IListUpdater, UpdateValue> : MonoBehaviour
        where IListUpdater : Component, IListUpdater<UpdateValue>
    {
        #region fields & properties
        public SingleItemList<IListUpdater, UpdateValue> ItemList => itemList;
        [SerializeField] private SingleItemList<IListUpdater, UpdateValue> itemList;
        #endregion fields & properties

        #region methods
        protected virtual void OnEnable()
        {
            UpdateListData();
        }
        protected virtual void OnDisable()
        {

        }
        protected abstract UpdateValue GetData();
        public void UpdateListData()
        {
            itemList.UpdateListData(GetData());
        }
        #endregion methods
    }
}