using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class ResourceGroup<ResourceInfoSO, ResourceInfo> : DBInfo
        where ResourceInfoSO : DataBase.ResourceInfoSO<ResourceInfo>
        where ResourceInfo : DataBase.ResourceInfo
    {
        #region fields & properties
        public IReadOnlyList<ResourceInfoSO> Group => group;
        [SerializeField] private ResourceInfoSO[] group;
        #endregion fields & properties

        #region methods
        public bool HasInfo(int id)
        {
            int l = group.Length;
            for (int i = 0; i < l; ++i)
            {
                if (group[i].Id == id) return true;
            }
            return false;
        }
        #endregion methods
    }
}