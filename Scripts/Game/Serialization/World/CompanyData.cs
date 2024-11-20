using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class CompanyData
    {
        #region fields & properties
        public UnityAction OnCreated;

        public RatingData Rating => rating;
        [SerializeField] private RatingData rating = new();
        public string FoundationDate => foundationDate;
        [SerializeField] private string foundationDate = string.Empty;
        public string Name => name;
        [SerializeField] private string name = string.Empty;
        public bool IsCreated => isCreated;
        [SerializeField] private bool isCreated = false;

        public ConstructionTasksData ConstructionTasks => constructionTasks;
        [SerializeField] private ConstructionTasksData constructionTasks = new();
        public WarehouseData WarehouseData => warehouseData;
        [SerializeField] private WarehouseData warehouseData = new(-1);
        public OfficeData OfficeData => officeData;
        [SerializeField] private OfficeData officeData = new(-1);
        public LandPlotsData LandPlotsData => landPlotsData;
        [SerializeField] private LandPlotsData landPlotsData = new();

        public ReviewsData ReviewsData => reviewsData;
        [SerializeField] private ReviewsData reviewsData = new();
        #endregion fields & properties

        #region methods
        public bool TryCreateCompany(string companyName)
        {
            if (IsCreated) return false;
            this.name = companyName;
            this.foundationDate = DateTime.Now.ToString("d");
            isCreated = true;
            OnCreated?.Invoke();
            return true;
        }
        #endregion methods
    }
}