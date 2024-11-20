using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ReviewItemList : InfinityFilteredItemListBase<ReviewItem, ReviewData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.CompanyData.ReviewsData.OnReviewAdded += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.CompanyData.ReviewsData.OnReviewAdded -= UpdateListData;
        }
        private void UpdateListData(ReviewData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ReviewData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<ReviewData> reviews = GameData.Data.CompanyData.ReviewsData.Reviews;
            int count = reviews.Count;
            for (int i = 0; i < count; ++i)
            {
                currentItemsReference.Add(reviews[i]);
            }
        }
        #endregion methods
    }
}