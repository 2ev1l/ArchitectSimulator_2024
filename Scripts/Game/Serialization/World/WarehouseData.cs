using Game.DataBase;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class WarehouseData : RentablePremiseData
    {
        #region fields & properties
        public UnityAction<float> OnSpaceChanged;
        /// <summary>
        /// Invokes only for new entries
        /// </summary>
        public UnityAction<ResourceData> OnAnyResourceAdded;
        /// <summary>
        /// Invokes only if resource is completely exhausted
        /// </summary>
        public UnityAction<ResourceData> OnAnyResourceRemoved;
        public override string BillDescription => LanguageInfo.GetTextByType(TextType.Game, 191);
        public double FreeSpace
        {
            get
            {
                WarehouseInfo info = ((WarehouseInfo)Info);
                if (info == null) return -1;
                return (double)info.Space - (double)OccupiedSpace;
            }
        }
        /// <summary>
        /// O(1) instead of Resources.Sum
        /// </summary>
        public float OccupiedSpace
        {
            get
            {
                TryFixOccupiedSpace();
                return occupiedSpace;
            }
            private set => SetOccupiedSpace(value);
        }
        [System.NonSerialized] private float occupiedSpace = -1;
        public IReadOnlyResources<ConstructionResourceData> ConstructionResources => constructionResources;
        [SerializeField] private ConstructionResourcesWarehouseData constructionResources = new();
        #endregion fields & properties

        #region methods
        protected override RentablePremise GetRentablePremiseInfo() => DB.Instance.RentableWarehouseInfo.Find(x => x.Data.PremiseInfo.Id == Id).Data;
        public override bool CanReplaceInfo(int newInfoId)
        {
            if (!base.CanReplaceInfo(newInfoId)) return false;
            if (newInfoId == -1) return OccupiedSpace == 0;
            WarehouseInfo newInfo = DB.Instance.WarehouseInfo[newInfoId].Data;
            return (newInfo.Space - OccupiedSpace) >= 0;
        }
        public IEnumerable<ResourceData> GetAllResources()
        {
            IEnumerable<ResourceData> result = constructionResources.Items;

            return result;
        }
        public bool TryAddConstructionResources(IEnumerable<ConstructionResourceData> resources) => TryAddResourcesTo(constructionResources, resources);
        public bool TryAddConstructionResource(ConstructionResourceData resource) => TryAddResourceTo(constructionResources, resource);
        public bool TryAddConstructionResource(int id, int count) => TryAddResourceTo(constructionResources, id, count);
        public void RemoveConstructionResource(int id, int count) => RemoveResourceFrom(constructionResources, id, count);
        public void RemoveConstructionResource(ConstructionResourceData existsResource, int count) => RemoveResourceFrom(constructionResources, existsResource, count);


        private void TryFixOccupiedSpace()
        {
            if (occupiedSpace >= 0) return;
            float newSpace = 0;
            foreach (ResourceData resource in GetAllResources())
            {
                newSpace += resource.GetTotalVolumeM3();
            }
            occupiedSpace = newSpace;
        }
        private void SetOccupiedSpace(float value)
        {
            TryFixOccupiedSpace();
            value = Mathf.Max(value, 0);
            occupiedSpace = value;
            OnSpaceChanged?.Invoke(occupiedSpace);
        }
        public bool CanAddResource(float resourceVolume) => resourceVolume <= FreeSpace;
        /// <summary>
        /// All or nothing
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="resourcesVolume"></param>
        /// <returns></returns>
        public bool CanAddResources(IEnumerable<ResourceData> resources, out float resourcesVolume)
        {
            resourcesVolume = 0;
            foreach (var el in resources)
            {
                resourcesVolume += el.GetTotalVolumeM3();
            }
            return CanAddResource(resourcesVolume);
        }

        private bool TryAddResourcesTo<T>(ResourcesWarehouseData<T> resources, IEnumerable<T> list) where T : ResourceData
        {
            if (!CanAddResources(list, out _)) return false;
            foreach (var el in list)
            {
                AddResourceTo(resources, el, el.GetTotalVolumeM3());
            }
            return true;
        }
        private bool TryAddResourceTo<T>(ResourcesWarehouseData<T> resources, int id, int count) where T : ResourceData
        {
            T newRes = resources.GetNewFixedResource(id, count);
            return TryAddResourceTo(resources, newRes);
        }
        private bool TryAddResourceTo<T>(ResourcesWarehouseData<T> resources, T resource) where T : ResourceData
        {
            float resourceVolume = resource.GetTotalVolumeM3();
            if (!CanAddResource(resourceVolume)) return false;
            AddResourceTo(resources, resource, resourceVolume);
            return true;
        }
        private void AddResourceTo<T>(ResourcesWarehouseData<T> resources, T resource, float resourceVolume) where T : ResourceData
        {
            int oldCount = resources.Items.Count;
            resources.AddResource(resource);
            int newCount = resources.Items.Count;
            if (oldCount != newCount)
            {
                OnAnyResourceAdded?.Invoke(resource);
            }
            OccupiedSpace += resourceVolume;
        }
        private void RemoveResourceFrom<T>(ResourcesWarehouseData<T> resources, int id, int count) where T : ResourceData
        {
            if (!resources.Exists(x => x.Id == id, out T exists)) return;
            RemoveResourceFrom(resources, exists, count);
        }
        private void RemoveResourceFrom<T>(ResourcesWarehouseData<T> resources, T existsResource, int count) where T : ResourceData
        {
            count = Mathf.Clamp(count, 0, existsResource.Count);
            if (count == 0) return;
            resources.RemoveResource(existsResource, count);
            OccupiedSpace -= count * existsResource.Info.Prefab.VolumeM3;

            if (!existsResource.IsRunOut) return;
            OnAnyResourceRemoved?.Invoke(existsResource);
        }

        public WarehouseData(int id) : base(id) { }
        #endregion methods
    }
}