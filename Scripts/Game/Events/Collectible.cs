using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Events
{
    public class Collectible : InteractableObject
    {
        #region fields & properties
        public int Id => id;
        [Title("Collectible")][SerializeField][Min(0)] private int id;
        public RewardInfo Reward => reward;
        [SerializeField] private RewardInfo reward;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            TryDeactivate();
        }
        private void TryDeactivate()
        {
            if (!GameData.Data.EnvironmentData.Collectibles.Exists(x => x == id, out _)) return;
            gameObject.SetActive(false);
        }
        protected override void OnInteract()
        {
            base.OnInteract();
            if (!GameData.Data.EnvironmentData.Collectibles.TryAddItem(id, x => x == id)) return;
            reward.AddReward();
            TryDeactivate();
        }
        #endregion methods

#if UNITY_EDITOR
        [Title("Debug")]
        [SerializeField][DontDraw] private bool ___testBool;

        [Button(nameof(DebugCollectibleCount))]
        private void DebugCollectibleCount()
        {
            Debug.Log($"{FindObjectsByType<Collectible>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length}");
        }
        [Button(nameof(IsIdFree))]
        private void IsIdFree()
        {
            var collectibles = FindObjectsByType<Collectible>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            bool notFree = false;
            foreach (Collectible el in collectibles)
            {
                if (el == this) continue;
                if (el.id == id)
                {
                    notFree = true;
                    break;
                }
            }
            Debug.Log($"{(notFree ? "No" : "Yes")}");
        }
        [Button(nameof(SetFreeId))]
        private void SetFreeId()
        {
            var collectibles = FindObjectsByType<Collectible>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int maxId = -1;
            foreach (Collectible el in collectibles)
            {
                if (el == this) continue;
                if (maxId < el.id)
                    maxId = el.id;
            }
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Change id");
            id = maxId + 1;
            EditorUtility.SetDirty(this);
        }
#endif //UNITY_EDITOR

    }
}