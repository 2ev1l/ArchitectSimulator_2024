using DebugStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic.Filters;

namespace Universal.Collections.Filters
{
    [System.Serializable]
    public class VirtualPageItemFilter<T> : VirtualFiltersBase<T>
    {
        #region fields & properties
        public UnityAction OnUpdateRequested
        {
            get => filter.OnUpdateRequested;
            set => filter.OnUpdateRequested = value;
        }
        [SerializeField] private PageItemsFilter filter;
        private readonly List<T> filteredItems = new();
        #endregion fields & properties

        #region methods
        protected override IEnumerable<T> ApplyMainFilters(IEnumerable<T> newList)
        {
            if (!filter.VirtualFilter.CanBeApplied)
                return base.ApplyMainFilters(newList);
            DoFilter(newList, out int newListCount);
            if (filteredItems.Count == 0 && filter.ItemsCount > 0 && newListCount > 0)
            {
                int maxPage = newListCount / filter.ItemsCount;
                if (newListCount % filter.ItemsCount == 0)
                    maxPage--;
                filter.SetCurrentPage(maxPage);
                DoFilter(newList, out int _);
            }
            return base.ApplyMainFilters(filteredItems);
        }
        private void DoFilter(IEnumerable<T> newList, out int newListCount)
        {
            filteredItems.Clear();
            filter.UpdateFilterData();
            int i = 0;
            foreach (var item in newList)
            {
                if (filter.FilterItem(i))
                    filteredItems.Add(item);
                i++;
            }
            newListCount = i;
        }
        #endregion methods
    }
}