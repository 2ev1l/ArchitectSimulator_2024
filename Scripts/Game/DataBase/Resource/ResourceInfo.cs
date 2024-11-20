using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public abstract class ResourceInfo : DBInfo, INameHandler
    {
        #region fields & properties
        public ResourcePrefab Prefab => prefab;
        [SerializeField] private ResourcePrefab prefab;
        public virtual LanguageInfo NameInfo => nameInfo;
        [SerializeField] private LanguageInfo nameInfo = new(0, TextType.Resource);
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}