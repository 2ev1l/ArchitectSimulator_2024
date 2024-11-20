using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [CreateAssetMenu(fileName = "ResourceGroupSO", menuName = "ScriptableObjects/ResourceGroupSO")]
    public class ResourceGroupSO<ResourceGroup, ResourceInfoSO, ResourceInfo> : DBScriptableObject<ResourceGroup> 
        where ResourceGroup : DataBase.ResourceGroup<ResourceInfoSO, ResourceInfo>
        where ResourceInfoSO : DataBase.ResourceInfoSO<ResourceInfo>
        where ResourceInfo : DataBase.ResourceInfo
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}