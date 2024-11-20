using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ReviewsData
    {
        #region fields & properties
        public UnityAction<ReviewData> OnReviewAdded;
        public IReadOnlyList<ReviewData> Reviews => reviews;
        [SerializeField] private List<ReviewData> reviews = new();
        #endregion fields & properties

        #region methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Null if task not added</returns>
        public ReviewData TryAdd(ConstructionTaskData task)
        {
            if (task == null) return null;
            int taskId = task.Id;
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            if (task.Result == null)
            {
                if (!tasks.IsTaskExpired(taskId, out _)) return null;
            }
            if (tasks.TaskCountInLists(taskId) > 1)
            {
                return null;
            }
            //todo check if tasks count > 1 than 
            ReviewData review = new(task);
            reviews.Add(review);
            OnReviewAdded?.Invoke(review);
            return review;
        }
        #endregion methods
    }
}