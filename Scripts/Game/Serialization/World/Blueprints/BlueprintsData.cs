using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BlueprintsData
    {
        #region fields & properties
        public UnityAction<BlueprintData> OnBlueprintAdded;
        public UnityAction<BlueprintData> OnBlueprintRemoved;
        /// <summary>
        /// Unique entries list by name and building reference
        /// </summary>
        public IReadOnlyList<BlueprintData> Blueprints => blueprints;
        [SerializeField] private List<BlueprintData> blueprints = new();
        [SerializeField][Min(-1)] private int totalBlueprintsAdded = -1;
        #endregion fields & properties

        #region methods
        public bool TryGetByBaseId(int baseId, out BlueprintData data)
        {
            data = null;
            if (baseId < 0) return false;
            int constructionsCount = blueprints.Count;
            for (int i = 0; i < constructionsCount; ++i)
            {
                BlueprintData bd = blueprints[i];
                if (bd.BaseId == baseId)
                {
                    data = bd;
                    return true;
                }
            }
            return false;
        }
        public bool TryRemoveBlueprint(int baseId)
        {
            if (baseId < 0) return false;
            if (!TryGetByBaseId(baseId, out BlueprintData exist)) return false;
            RemoveBlueprint(exist);
            return true;
        }
        public bool TryRemoveBlueprint(string name)
        {
            if (!blueprints.Exists(x => x.Name.Equals(name), out BlueprintData exist)) return false;
            RemoveBlueprint(exist);
            return true;
        }
        private void RemoveBlueprint(BlueprintData blueprint)
        {
            blueprints.Remove(blueprint);
            OnBlueprintRemoved?.Invoke(blueprint);
        }
        public bool TryAddNewBlueprint(BlueprintData blueprint) => TryAddNewBlueprint(blueprint, out int _);
        public bool TryAddNewBlueprint(BlueprintData blueprint, out string lockReason)
        {
            lockReason = "";
            if (!TryAddNewBlueprint(blueprint, out int lockReasonId))
            {
                lockReason = LanguageInfo.GetTextByType(TextType.Game, lockReasonId);
                return false;
            }
            return true;
        }
        private bool TryAddNewBlueprint(BlueprintData blueprint, out int lockReasonGameTextId)
        {
            int count = blueprints.Count;
            int infoReference = blueprint.BlueprintInfoId;
            string name = blueprint.Name;
            lockReasonGameTextId = -1;
            for (int i = 0; i < count; ++i)
            {
                BlueprintData blueprintData = blueprints[i];
                if (blueprintData.Name.Equals(name))
                {
                    lockReasonGameTextId = 144;
                    return false;
                }
            }
            totalBlueprintsAdded++;
            blueprint.ChangeBaseId(totalBlueprintsAdded);
            blueprints.Add(blueprint);
            OnBlueprintAdded?.Invoke(blueprint);
            return true;
        }
        public bool ContainName(string name)
        {
            int count = blueprints.Count;
            for (int i = 0; i < count; ++i)
            {
                if (blueprints[i].Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion methods
    }
}