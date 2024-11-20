using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;

namespace Game.Environment
{
    public class OfficeStateChange : StateChange
    {
        #region fields & properties
        public GameObject Root => root;
        [SerializeField] private GameObject root;
        public Transform SafePosition => safePosition;
        [SerializeField] private Transform safePosition;
        public int OfficeId => officeId;
        [SerializeField][Min(0)] private int officeId = 0;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            if (root.activeSelf != active)
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