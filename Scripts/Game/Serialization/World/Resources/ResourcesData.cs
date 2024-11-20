using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ResourcesData<T> : IReadOnlyResources<T> where T : ResourceData
    {
        #region fields & properties
        public UnityAction<T> OnResourceAdded { get; set; }
        public UnityAction<T> OnResourceRemoved { get; set; }
        public IReadOnlyList<T> Items => data.Items;
        [SerializeField] private UniqueList<T> data = new();
        #endregion fields & properties

        #region methods
        public bool Exists(System.Predicate<T> match, out T item) => data.Exists(match, out item);
        protected abstract T GetNewResource(int id);
        public T GetNewFixedResource(int id, int count)
        {
            T res = GetNewResource(id);
            FixNewResourceCount(res, count);
            return res;
        }
        public void AddResources(IEnumerable<T> resources)
        {
            foreach (var el in resources)
            {
                AddResource(el);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="newResource">May be not the same as added in data</param>
        /// <returns></returns>
        public void AddResource(int id, int count, out T newResource)
        {
            newResource = GetNewFixedResource(id, count);
            AddResource(newResource);
        }

        public void AddResource(T resource)
        {
            if (data.TryAddItem(resource, x => x.Id == resource.Id, out T exists))
            {
                OnResourceAdded?.Invoke(resource);
                return;
            }

            //if resource exist than modify its own data
            exists.Add(resource.Count);
        }
        public void RemoveResource(int id, int count)
        {
            if (!data.Exists(x => x.Id == id, out T exists)) return;
            RemoveResource(exists, count);
        }
        public void RemoveResource(T existsResource, int count)
        {
            if (count == 0) return;
            existsResource.Remove(ref count);

            if (!existsResource.IsRunOut) return;
            data.RemoveItem(existsResource);
            OnResourceRemoved?.Invoke(existsResource);
        }

        private void FixNewResourceCount(T newResource, int count)
        {
            //resource creates with 1 count
            newResource.Add(count - 1);
        }
        #endregion methods
    }
}