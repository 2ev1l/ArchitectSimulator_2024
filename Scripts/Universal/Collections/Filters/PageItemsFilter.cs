using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Collections.Generic.Filters;

namespace Universal.Collections.Filters
{
    public class PageItemsFilter : VirtualFilter, ISmartFilter<int>
    {
        #region fields & properties
        public UnityAction OnUpdateRequested;
        public VirtualFilter VirtualFilter => this;
        [SerializeField] private TMP_InputField pageInput;
        [SerializeField] private TMP_InputField itemsCountInput;
        public int Page => page;
        [SerializeField][Min(0)] private int page = 0;
        public int ItemsCount => itemsCount;
        [SerializeField][Min(1)] private int itemsCount = 1;
        private int minAllowedIndex = 0;
        private int maxAllowedIndex = 0;
        #endregion fields & properties

        #region methods
        private void OnDestroy()
        {
            OnUpdateRequested = null;
        }
        /// <summary>
        /// Invokes no matter the filter is applied
        /// </summary>
        [SerializedMethod]
        public void ForceRequestUpdateList()
        {
            OnUpdateRequested?.Invoke();
        }

        /// <summary>
        /// Invokes only if filter can be applied
        /// </summary>
        [SerializedMethod]
        public void RequestUpdateList()
        {
            if (!CanBeApplied) return;
            ForceRequestUpdateList();
        }
        [SerializedMethod]
        public void DecreasePage()
        {
            SetCurrentPage(page - 1);
            RequestUpdateList();
        }
        [SerializedMethod]
        public void IncreasePage()
        {
            SetCurrentPage(page + 1);
            RequestUpdateList();
        }
        [SerializedMethod]
        public void ReadInputPage(string page)
        {
            SetCurrentPage(page);
            RequestUpdateList();
        }
        [SerializedMethod]
        public void ReadInputItemsCount(string itemsCount)
        {
            SetItemsPerPage(itemsCount);
            RequestUpdateList();
        }

        public void SetItemsPerPage(string itemsCount)
        {
            int itemsCountI = 0;
            try { itemsCountI = System.Convert.ToInt32(itemsCount); }
            catch { }
            SetItemsPerPage(itemsCountI);
        }
        public void SetItemsPerPage(int itemsCount)
        {
            itemsCount = Mathf.Clamp(itemsCount, 1, 999);
            this.itemsCount = itemsCount;
            if (itemsCountInput != null) itemsCountInput.text = $"{this.itemsCount}";
        }
        public void SetCurrentPage(string page)
        {
            int pageI = 0;
            try { pageI = System.Convert.ToInt32(page); }
            catch { }
            SetCurrentPage(pageI);
        }
        public void SetCurrentPage(int page)
        {
            page = Mathf.Clamp(page, 0, 99);
            this.page = page;
            if (pageInput != null) pageInput.text = $"{this.page}";
        }
        public void UpdateFilterData()
        {
            if (itemsCountInput != null)
                SetItemsPerPage(itemsCountInput.text);
            if (pageInput != null)
                SetCurrentPage(pageInput.text);
            minAllowedIndex = page * itemsCount;
            maxAllowedIndex = (page + 1) * itemsCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">Index in list</param>
        /// <returns></returns>
        public bool FilterItem(int item)
        {
            if (item < minAllowedIndex || item >= maxAllowedIndex) return false;
            return true;
        }
        #endregion methods
    }
}