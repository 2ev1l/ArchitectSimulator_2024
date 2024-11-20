using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;

namespace Game.Environment
{
    public class HouseStateChange : StateChange
    {
        #region fields & properties
        public GameObject Root => root;
        [SerializeField] private GameObject root;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            if (root.activeSelf == active) return;
            root.SetActive(active);
        }
        protected virtual void OnValidate()
        {
            if (root == null)
                root = gameObject;
        }
        #endregion methods
    }
}