using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BrowserData
    {
        #region fields & properties
        public ConstructionResourceShopCartData ConstructionResourceShop => constructionResourceShop;
        [SerializeField] private ConstructionResourceShopCartData constructionResourceShop = new();

        public RentableOfficeShopCartData OfficeShop => officeShop;
        [SerializeField] private RentableOfficeShopCartData officeShop = new();
        public RentableWarehouseShopCartData WarehouseShop => warehouseShop;
        [SerializeField] private RentableWarehouseShopCartData warehouseShop = new();
        public RentableLandPlotShopCartData LandPlotShop => landPlotShop;
        [SerializeField] private RentableLandPlotShopCartData landPlotShop = new();

        public RecruitBuilderShopCartData BuildersRecruit => buildersRecruit;
        [SerializeField] private RecruitBuilderShopCartData buildersRecruit = new();
        public RecruitHRShopCartData HRRecruit => hrRecruit;
        [SerializeField] private RecruitHRShopCartData hrRecruit = new();
        public RecruitPRShopCartData PRRecruit => prRecruit;
        [SerializeField] private RecruitPRShopCartData prRecruit = new();
        public RecruitDesignEngineerShopCartData DesignEngineerRecruit => designEngineerRecruit;
        [SerializeField] private RecruitDesignEngineerShopCartData designEngineerRecruit = new();

        public HouseShopCartData HouseShop => houseShop;
        [SerializeField] private HouseShopCartData houseShop = new();
        public FoodShopCartData FoodShop => foodShop;
        [SerializeField] private FoodShopCartData foodShop = new();
        public VehicleShopCartData VehicleShop => vehicleShop;
        [SerializeField] private VehicleShopCartData vehicleShop = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}