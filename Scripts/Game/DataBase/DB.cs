using UnityEngine;
using EditorCustom.Attributes;
using Universal.Core;
using System.Linq;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

namespace Game.DataBase
{
    [ExecuteAlways]
    public class DB : MonoBehaviour
    {
        #region fields & properties
        public static DB Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) //only in editor
                    return GameObject.FindFirstObjectByType<DB>(FindObjectsInactive.Include);
#endif //UNITY_EDITOR
                return instance;
            }
            set
            {
                if (instance == null)
                    instance = value;
            }
        }
        private static DB instance;
        public DBSOSet<MoodInfoSO> MoodInfo => moodInfo;
        [SerializeField] private DBSOSet<MoodInfoSO> moodInfo;
        public DBSOSet<PlayerTaskInfoSO> PlayerTaskInfo => playerTaskInfo;
        [SerializeField] private DBSOSet<PlayerTaskInfoSO> playerTaskInfo;
        public DBSOSet<ConstructionTaskInfoSO> ConstructionTaskInfo => constructionTaskInfo;
        [SerializeField] private DBSOSet<ConstructionTaskInfoSO> constructionTaskInfo;

        public DBSOSet<ConstructionResourceInfoSO> ConstructionResourceInfo => constructionResourceInfo;
        [SerializeField] private DBSOSet<ConstructionResourceInfoSO> constructionResourceInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<BuyableConstructionResourceSO> BuyableConstructionResourceInfo => buyableConstructionResourceInfo;
        [SerializeField] private DBSOSet<BuyableConstructionResourceSO> buyableConstructionResourceInfo;
        public DBSOSet<ConstructionResourceGroupSO> ConstructionResourceGroups => constructionResourceGroups;
        [SerializeField] private DBSOSet<ConstructionResourceGroupSO> constructionResourceGroups;


        public DBSOSet<WarehouseInfoSO> WarehouseInfo => warehouseInfo;
        [SerializeField] private DBSOSet<WarehouseInfoSO> warehouseInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<RentableWarehouseSO> RentableWarehouseInfo => rentableWarehouseInfo;
        [SerializeField] private DBSOSet<RentableWarehouseSO> rentableWarehouseInfo;

        public DBSOSet<OfficeInfoSO> OfficeInfo => officeInfo;
        [SerializeField] private DBSOSet<OfficeInfoSO> officeInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<RentableOfficeSO> RentableOfficeInfo => rentableOfficeInfo;
        [SerializeField] private DBSOSet<RentableOfficeSO> rentableOfficeInfo;

        public DBSOSet<LandPlotInfoSO> LandPlotInfo => landPlotInfo;
        [SerializeField] private DBSOSet<LandPlotInfoSO> landPlotInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<RentableLandPlotSO> RentableLandPlotInfo => rentableLandPlotInfo;
        [SerializeField] private DBSOSet<RentableLandPlotSO> rentableLandPlotInfo;

        public DBSOSet<HouseInfoSO> HouseInfo => houseInfo;
        [SerializeField] private DBSOSet<HouseInfoSO> houseInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<RentableHouseSO> RentableHouseInfo => rentableHouseInfo;
        [SerializeField] private DBSOSet<RentableHouseSO> rentableHouseInfo;

        public DBSOSet<BlueprintInfoSO> BlueprintInfo => blueprintInfo;
        [SerializeField] private DBSOSet<BlueprintInfoSO> blueprintInfo;

        public HumanInfoSOSet HumanInfo => humanInfo;
        [SerializeField] private HumanInfoSOSet humanInfo;

        public DBSOSet<FoodInfoSO> FoodInfo => foodInfo;
        [SerializeField] private DBSOSet<FoodInfoSO> foodInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<BuyableFoodSO> BuyableFoodInfo => buyableFoodInfo;
        [SerializeField] private DBSOSet<BuyableFoodSO> buyableFoodInfo;

        public DBSOSet<VehicleInfoSO> VehicleInfo => vehicleInfo;
        [SerializeField] private DBSOSet<VehicleInfoSO> vehicleInfo;
        /// <summary>
        /// Don't use indexer
        /// </summary>
        public DBSOSet<BuyableVehicleSO> BuyableVehicleInfo => buyableVehicleInfo;
        [SerializeField] private DBSOSet<BuyableVehicleSO> buyableVehicleInfo;

        #region optimization
        [System.NonSerialized] private List<IMinimalRatingHandler> _minRatingHandlers = null;
        #endregion optimization
        #endregion fields & properties

        #region methods
        [Todo("Add IMinimalRatingHandler")]
        public IReadOnlyList<IMinimalRatingHandler> GetMinRatingHandlers()
        {
            _minRatingHandlers ??= new();
            _minRatingHandlers.AddRange(ConstructionTaskInfo.Data.Select(x => x.Data));
            _minRatingHandlers.AddRange(RentableOfficeInfo.Data.Select(x => x.Data));
            _minRatingHandlers.AddRange(RentableLandPlotInfo.Data.Select(x => x.Data));
            _minRatingHandlers.AddRange(RentableWarehouseInfo.Data.Select(x => x.Data));

            return _minRatingHandlers;
        }
        #endregion methods

#if UNITY_EDITOR
        [SerializeField] private bool automaticallyUpdateEditor = false;
        private void OnValidate()
        {
            if (!automaticallyUpdateEditor) return;
            GetAllDBInfo();
            CheckAllErrors();
        }
        /// <summary>
        /// You need to manually add code
        /// </summary>
        [Button(nameof(GetAllDBInfo))]
        private void GetAllDBInfo()
        {
            if (Application.isPlaying) return;
            AssetDatabase.Refresh();
            Undo.RegisterCompleteObjectUndo(this, "Update DB");

            //call dbset.CollectAll()
            moodInfo.CollectAll();
            playerTaskInfo.CollectAll();
            constructionTaskInfo.CollectAll();
            constructionResourceInfo.CollectAll();
            constructionResourceGroups.CollectAll();
            buyableConstructionResourceInfo.CollectAll();
            warehouseInfo.CollectAll();
            rentableWarehouseInfo.CollectAll();
            officeInfo.CollectAll();
            rentableOfficeInfo.CollectAll();
            landPlotInfo.CollectAll();
            rentableLandPlotInfo.CollectAll();
            blueprintInfo.CollectAll();
            humanInfo.CollectAll();
            houseInfo.CollectAll();
            rentableHouseInfo.CollectAll();
            foodInfo.CollectAll();
            buyableFoodInfo.CollectAll();
            vehicleInfo.CollectAll();
            buyableVehicleInfo.CollectAll();

            EditorUtility.SetDirty(this);
        }
        /// <summary>
        /// You need to manually add code
        /// </summary>
        [Button(nameof(CheckAllErrors))]
        private void CheckAllErrors()
        {
            if (!Application.isPlaying) return;
            //call dbset.CatchExceptions(x => ...)
            System.Exception e = new();

            moodInfo.CatchExceptions(x => _ = x.Data.Sprite == null ? throw e : 0, "Sprite must be not null");

            CatchTaskReferenceExceptions<PlayerTaskInfoSO, PlayerTaskInfo>(playerTaskInfo);
            playerTaskInfo.CatchExceptions(x => x.Data.NextTasksTrigger.ExistsEquals((x, y) => x == y), e, "Next tasks must be unique");
            playerTaskInfo.CatchExceptions(x => x.Data.StartSubtitlesTrigger.ExistsEquals((x, y) => x == y), e, "Subtitles must be unique");
            playerTaskInfo.CatchExceptions(x => x.Data.StartSubtitlesTrigger.ToList().ForEach(id => LanguageInfo.GetTextByType(TextType.Subtitle, id)), "Wrong subtitle ids");
            playerTaskInfo.CatchExceptions(x => x.Data.NextTasksTrigger.Exists(id => id > playerTaskInfo.Data.Count - 1, out _), e, "Next Task id doesn't exist in db");

            constructionTaskInfo.CatchExceptions(x => !x.Data.RewardInfo.Rewards.Exists(x => x.Type == RewardType.Money, out _), e, "Task must contain money reward");
            constructionTaskInfo.CatchExceptions(x => !((int)x.Data.ResourcesType).IsPowerOfTwo() && ((int)x.Data.ResourcesType) != 0, e, "Resource type must not be multiple");
            constructionTaskInfo.CatchExceptions(x => _ = HumanInfo[x.Data.HumanId], "Human info is wrong");
            constructionTaskInfo.CatchExceptions(x => x.Data.ClarificationPrice < 0, e, "Clarification price must be >= 0");
            constructionTaskInfo.CatchExceptions(x => x.Data.MonthDuration == 1, e, "Month duration must not be '1'");
            CatchTaskReferenceExceptions<ConstructionTaskInfoSO, ConstructionTaskInfo>(constructionTaskInfo);
            CatchMaximalRatingHandlerExceptions<ConstructionTaskInfoSO, ConstructionTaskInfo>(constructionTaskInfo);

            CatchBlueprintReferenceExceptions<ConstructionTaskInfoSO, ConstructionTaskInfo>(constructionTaskInfo);

            CatchConstructionResourceExceptions();
            CatchResourceGroupsExceptions<ConstructionResourceGroupSO, ConstructionResourceGroup, ConstructionResourceInfoSO, ConstructionResourceInfo>(constructionResourceGroups);

            CatchPremiseReferenceExceptions<WarehouseInfoSO, WarehouseInfo>(warehouseInfo);
            CatchPremiseReferenceExceptions<OfficeInfoSO, OfficeInfo>(officeInfo);
            CatchPremiseReferenceExceptions<LandPlotInfoSO, LandPlotInfo>(landPlotInfo);
            landPlotInfo.CatchExceptions(x => x.Data.BuildedSprite == null, e, "Builded sprite required");

            CatchBuyableObjectExceptions<BuyableConstructionResourceSO, BuyableConstructionResource>(buyableConstructionResourceInfo);
            CatchMinimalRatingHandlerExceptions<BuyableConstructionResourceSO, BuyableConstructionResource>(buyableConstructionResourceInfo);
            CatchRentableObjectExceptions<RentableWarehouseSO, RentableWarehouse>(rentableWarehouseInfo);
            CatchMinimalRatingHandlerExceptions<RentableWarehouseSO, RentableWarehouse>(rentableWarehouseInfo);
            CatchRentableObjectExceptions<RentableOfficeSO, RentableOffice>(rentableOfficeInfo);
            CatchMinimalRatingHandlerExceptions<RentableOfficeSO, RentableOffice>(rentableOfficeInfo);
            CatchRentableObjectExceptions<RentableLandPlotSO, RentableLandPlot>(rentableLandPlotInfo);
            CatchMinimalRatingHandlerExceptions<RentableLandPlotSO, RentableLandPlot>(rentableLandPlotInfo);
            CatchRentableObjectExceptions<RentableHouseSO, RentableHouse>(rentableHouseInfo);
            CatchMinimalRatingHandlerExceptions<RentableHouseSO, RentableHouse>(rentableHouseInfo);

            warehouseInfo.CatchExceptions(x => x.Data.Space < 0, e, "Space must be >= 0");
            officeInfo.CatchExceptions(x => x.Data.MaximumEmployees <= 0, e, "Max employees must be > 0");
            CatchBlueprintReferenceExceptions<LandPlotInfoSO, LandPlotInfo>(landPlotInfo);

            CatchBlueprintExceptions();

            CatchPreviewHandlerExceptions<HumanInfoSO, HumanInfo>(humanInfo);
            humanInfo.CatchExceptions(x => x.Data.Name.Length <= 1, e, "Name length must be > 1");

            CatchRealEstateReferenceExceptions<HouseInfoSO, HouseInfo>(houseInfo);
            houseInfo.CatchExceptions(x => x.Data.MaxPeople < 1, e, "Max people must be > 0");

            CatchPreviewHandlerExceptions<FoodInfoSO, FoodInfo>(foodInfo);
            CatchNameHandlerExceptions<FoodInfoSO, FoodInfo>(foodInfo);
            foodInfo.CatchExceptions(x => x.Data.Saturation < 1, e, "Saturation must be > 0");

            CatchBuyableObjectExceptions<BuyableFoodSO, BuyableFood>(buyableFoodInfo);

            CatchPreviewHandlerExceptions<VehicleInfoSO, VehicleInfo>(vehicleInfo);
            CatchMoodScaleHandlerExceptions<VehicleInfoSO, VehicleInfo>(vehicleInfo);
            vehicleInfo.CatchExceptions(x => x.Data.Name.Length < 1, e, "Name length must be > 0");
            vehicleInfo.CatchExceptions(x => x.Data.HousePrefab == null, e, "House Prefab must exist");
            vehicleInfo.CatchExceptions(x => x.Data.CityPrefab == null, e, "City Prefab must exist");

            CatchBuyableObjectExceptions<BuyableVehicleSO, BuyableVehicle>(buyableVehicleInfo);
        }
        private void CatchResourceGroupsExceptions<SO, RG, RISO, RI>(DBSOSet<SO> dbset)
            where SO : ResourceGroupSO<RG, RISO, RI>
            where RG : ResourceGroup<RISO, RI>
            where RISO : ResourceInfoSO<RI>
            where RI : ResourceInfo
        {
            System.Exception e = new();
            dbset.CatchDefaultExceptions();
            foreach (var x in dbset.Data)
            {
                int c = x.Data.Group.Count;
                for (int i = 0; i < c; ++i)
                {
                    int iId = x.Data.Group[i].Id;
                    for (int j = i + 1; j < c - 1; ++j)
                    {
                        int jId = x.Data.Group[j].Id;
                        if (iId == jId)
                        {
                            Debug.LogError($"Same ids #{iId} in Group #{x.Data.Id} at {i} & {j} positions", x);
                        }
                    }
                }
            }

            dbset.Data.FindSame((x, y) =>
            {
                if (x.Id == y.Id) return false;
                foreach (var xEl in x.Data.Group)
                {
                    foreach (var yEl in y.Data.Group)
                    {
                        if (xEl.Id == yEl.Id)
                        {
                            Debug.LogError($"Same id #{xEl.Id} in Group #{x.Data.Id} & #{y.Data.Id}", x);
                        }
                    }
                }
                return false;
            });
        }
        private void CatchBlueprintExceptions()
        {
            System.Exception e = new();
            blueprintInfo.CatchExceptions(x => (int)x.Data.BuildingInfo.BuildingType == 0, e, "Building type must not be Unknown");
            blueprintInfo.CatchExceptions(x => !Mathf.IsPowerOfTwo((int)x.Data.BuildingInfo.BuildingType), e, "Building type must not be multiple flag");
            blueprintInfo.CatchExceptions(x => !Mathf.IsPowerOfTwo((int)x.Data.BuildingInfo.BuildingStyle), e, "Building style must not be multiple flag");
            blueprintInfo.CatchExceptions(x => x.Data.BuildingInfo.Floor2AdditionalCount < 0, e, "Additional floor count must be >= 0");
            blueprintInfo.CatchExceptions(x => x.Data.BuildingInfo.Floor2AdditionalCount > 20, e, "Additional floor count must be <= 20");
            blueprintInfo.CatchExceptions(x => x.Data.BuildingInfo.Floor2AdditionalCount > 0 && x.Data.BuildingInfo.MaxFloor != BuildingFloor.F3_Roof, e, "Additional floors allowed only for F3_R max floor");

            List<BuildingFloor> allowedFloorsForBuildingInfo = new() { BuildingFloor.F2_FlooringRoof, BuildingFloor.F3_Roof };
            blueprintInfo.CatchExceptions(x => !allowedFloorsForBuildingInfo.Contains(x.Data.BuildingInfo.MaxFloor), e, "Max floor must be F2_FR or F3_R");

            blueprintInfo.CatchExceptions(x => x.Data.BlueprintZones.Exists(x => x.FloorPlaced != BuildingFloor.F1_Flooring, out _), e, "Blueprint zones building floor must be F1_F");
            blueprintInfo.CatchExceptions(x => x.Data.BlueprintZones.Exists(x => x.Placement == PolygonBlueprintGraphic.PlacementType.Neutral, out _), e, "Blueprint zones placement must not be neutral");
            blueprintInfo.CatchExceptions(x => x.Data.BlueprintZones.Exists(x => x.TexturePoints.Count < 3, out _), e, "Blueprint zones texture points count must be >= 3");
            blueprintInfo.CatchExceptions(x => x.Data.BlueprintZones.Where(x => x.Placement == PolygonBlueprintGraphic.PlacementType.Good).Count() > 1, e, "Multiple blueprint zones with good placement are not allowed");

            blueprintInfo.CatchExceptions(x => x.Data.RoomsInfo.Exists(x => x.Floor != BuildingFloor.F1 && x.Floor != BuildingFloor.F2, out _), e, "Rooms must be placed on F1 or F2");
            blueprintInfo.CatchExceptions(x => x.Data.RoomsInfo.Exists(y => x.Data.BuildingInfo.MaxFloor == BuildingFloor.F2_FlooringRoof && y.Floor == BuildingFloor.F2, out _), e, "Rooms floor is higher than max");
            //blueprintInfo.CatchExceptions(x => x.Data.RoomsInfo.Count == 0, e, "At least 1 room must exist"); //0 rooms allowed
            blueprintInfo.CatchExceptions(x => x.Data.RoomsInfo.Count(y => y.Rooms.ExistsSame((a, b) => a.Room == b.Room)) > 0, e, "Rooms on the floor must be unique");
            blueprintInfo.CatchExceptions(x => x.Data.RoomsInfo.ExistsSame((a, b) => a.Floor == b.Floor), e, "Floors must be unique");

            List<IBlueprintHandler> blueprints = new();
            blueprints.AddRange(constructionTaskInfo.Data.Select(x => x.Data));
            blueprints.AddRange(landPlotInfo.Data.Select(x => x.Data));
            HashSet<IBlueprintHandler> sameBP = (blueprints.FindSame((x, y) => x.BlueprintInfo.Id == y.BlueprintInfo.Id));
            if (sameBP.Count > 0)
            {
                foreach (var el in sameBP)
                    Debug.LogError($"Same blueprint reference #{el.BlueprintInfo.Id}");
            }
        }
        private void CatchConstructionResourceExceptions()
        {
            System.Exception e = new();
            CatchResourceReferenceExceptions<ConstructionResourceInfoSO, ConstructionResourceInfo>(constructionResourceInfo);
            constructionResourceInfo.CatchExceptions(x => x.Data.Blueprint == null, e, "Check blueprint");
            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionType == ConstructionType.Wall &&
                    (x.Data.BuildingFloor.HasFlag(BuildingFloor.F2_FlooringRoof) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F1_Flooring) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F3_Roof)), e, "Check floor flags");
            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionType == ConstructionType.Floor &&
                    (x.Data.BuildingFloor.HasFlag(BuildingFloor.F1) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F2) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F3_Roof)), e, "Check floor flags");
            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionType == ConstructionType.Roof &&
                    (x.Data.BuildingFloor.HasFlag(BuildingFloor.F1) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F2) ||
                    x.Data.BuildingFloor.HasFlag(BuildingFloor.F1_Flooring)), e, "Check floor flags");

            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionType == ConstructionType.Floor &&
                    x.Data.ConstructionSubtype != ConstructionSubtype.Base, e, "Check subtype");

            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionType == ConstructionType.Roof &&
                    x.Data.ConstructionSubtype != ConstructionSubtype.Base, e, "Check subtype");

            constructionResourceInfo.CatchExceptions(x => x.Data.ConstructionSubtype == ConstructionSubtype.Staircase &&
                    !x.Data.BuildingFloor.HasFlag(BuildingFloor.F1), e, "Check floor flags");

            constructionResourceInfo.CatchExceptions(x => x.Data.Prefab.gameObject.layer != 8, e, "Wrong layer");
        }
        private void CatchRealEstateReferenceExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : RealEstateInfoSO<Data> where Data : RealEstateInfo
        {
            CatchNameHandlerExceptions<SO, Data>(dbset);
            CatchPreviewHandlerExceptions<SO, Data>(dbset);
            CatchMoodScaleHandlerExceptions<SO, Data>(dbset);
        }
        private void CatchNameHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, INameHandler
        {
            dbset.CatchExceptions(x => _ = x.Data.NameInfo, "Name info is not correct");
        }
        private void CatchDescriptionHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IDescriptionHandler
        {
            dbset.CatchExceptions(x => _ = x.Data.DescriptionInfo, "Description info is not correct");
        }
        private void CatchPreviewHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IPreviewHandler
        {
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.PreviewSprite == null, e, "Preview sprite must be not null");
        }
        private void CatchMoodScaleHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IMoodScaleHandler
        {
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.MoodScale <= 0f, e, "Mood scale must be > 0f");
        }
        private void CatchMinimalRatingHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IMinimalRatingHandler
        {
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.MinRating < 0 || x.Data.MinRating > 100, e, "Minimal rating is not correct");
        }
        private void CatchMaximalRatingHandlerExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IMaximalRatingHandler
        {
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.MaxRating <= 0 || x.Data.MaxRating > 100, e, "Maximal rating is not correct");
        }
        private void CatchBlueprintReferenceExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : DBScriptableObject<Data> where Data : DBInfo, IBlueprintHandler
        {
            HashSet<SO> sameBP = (dbset.Data.FindSame((x, y) => x.Data.BlueprintInfo.Id == y.Data.BlueprintInfo.Id));
            if (sameBP.Count > 0)
            {
                foreach (var el in sameBP)
                    Debug.LogError($"{el.name} : Same blueprint reference #{el.Data.BlueprintInfo.Id}", el);
            }
        }
        private void CatchTaskReferenceExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : TaskInfoSO<Data> where Data : TaskInfo
        {
            System.Exception e = new();
            CatchNameHandlerExceptions<SO, Data>(dbset);
            CatchDescriptionHandlerExceptions<SO, Data>(dbset);
            dbset.CatchExceptions(x => x.Data.RewardInfo.Rewards.Exists(x => (x.Value == 0 || x.Value > 10) && x.Type == RewardType.Rating, out _), e, "Check rewards value in rating type");
            dbset.CatchExceptions(x => x.Data.RewardInfo.Rewards.Exists(x => x.Value == 0 && x.Type == RewardType.Money, out _), e, "Check rewards value in money type");
            dbset.CatchExceptions(x => x.Data.RewardInfo.Rewards.Exists(x => (x.Value == 0 || x.Value > 100) && x.Type == RewardType.Mood, out _), e, "Check rewards value in mood type");
            dbset.CatchExceptions(x => x.Data.RewardInfo.Rewards.ExistsEquals((x, y) => x.Value == y.Value && x.Type == y.Type), e, "Rewards must be unique");
        }
        private void CatchPremiseReferenceExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : PremiseInfoSO<Data> where Data : PremiseInfo
        {
            CatchPreviewHandlerExceptions<SO, Data>(dbset);
            CatchNameHandlerExceptions<SO, Data>(dbset);
        }
        private void CatchRentableObjectExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : RentableObjectSO<Data> where Data : RentableObject
        {
            CatchBuyableObjectExceptions<SO, Data>(dbset);
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.RentPrice <= 0, e, "Rent price is not correct");
        }
        private void CatchBuyableObjectExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : BuyableObjectSO<Data> where Data : BuyableObject
        {
            System.Exception e = new();
            dbset.CatchExceptions(x => x.Data.MaxDiscount < 0 || x.Data.MaxDiscount > 90, e, "Max discount range isn't correct");
            dbset.CatchExceptions(x => x.Data.ObjectReference == null, e, "Object Reference must be not null");
            dbset.CatchExceptions(x => x.Data.Price <= 0, e, "Price is not correct");

            var same = dbset.Data.FindSame((x, y) => x.Data.ObjectReference == y.Data.ObjectReference);
            foreach (var el in same)
                Debug.LogError($"Object Reference in {el.name} must be unique", el);
        }
        private void CatchResourceReferenceExceptions<SO, Data>(DBSOSet<SO> dbset) where SO : ResourceInfoSO<Data> where Data : ResourceInfo
        {
            System.Exception e = new();
            CatchNameHandlerExceptions<SO, Data>(dbset);
            dbset.CatchExceptions(x => _ = x.Data.Prefab == null, e, "Prefab is null");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.Count == 0, e, "Materials count is zero");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.Exists(x => x.Materials.Count == 0, out _), e, "Materials must be > 0");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.ExistsEquals((x, y) => x.Color == y.Color), e, "Colors must be unique");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.Exists(y => y.ColorPreview.Exists(z => z == null, out _), out _), e, "Colors must be unique");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.Exists(y => y.ColorPreview == null, out _), e, "One of sprites is null");
            dbset.CatchExceptions(x => x.Data.Prefab.MaterialsInfo.Exists(y => y.Id != x.Data.Prefab.MaterialsInfoInternal.IndexOf(y), out _), e, "Color id isn't matching array index");
            var sameRI = dbset.Data.FindSame((x, y) => x.Data.Prefab == y.Data.Prefab);
            foreach (var el in sameRI)
                Debug.LogError($"Prefab in {el.name} must be unique", el);
        }

#endif //UNITY_EDITOR
    }
}