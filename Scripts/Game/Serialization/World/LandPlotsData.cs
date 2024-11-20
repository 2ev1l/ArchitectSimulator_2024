using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class LandPlotsData : IBillPayable, IMonthUpdatable
    {
        #region fields & properties
        public UnityAction<LandPlotData> OnPlotSold;
        public UnityAction<LandPlotData> OnPlotEndedSelling;
        public UnityAction<LandPlotData> OnPlotStartedSelling;
        public UnityAction<LandPlotData> OnPlotAdded;
        public UnityAction<LandPlotData> OnPlotBlueprintStarted;
        public UnityAction<LandPlotData> OnPlotBlueprintFinished;
        public UnityAction<LandPlotOfferData> OnOfferAdded;

        public bool CanAddBill => plots.Count > 0;
        public int BillPaymentAmount => CalculateBill();
        public string BillDescription => LandPlotData.BillDescriptionInternal;
        public IReadOnlyList<LandPlotData> Plots => plots;
        [SerializeField] private List<LandPlotData> plots = new();
        public IReadOnlyList<SellingLandPlotData> SellingPlots => sellingPlots;
        [SerializeField] private List<SellingLandPlotData> sellingPlots = new();
        public IReadOnlyList<SellingLandPlotData> SoldPlots => soldPlots;
        [SerializeField] private List<SellingLandPlotData> soldPlots = new();

        public IReadOnlyList<LandPlotOfferData> Offers => offers;
        [SerializeField] private List<LandPlotOfferData> offers = new();
        #endregion fields & properties

        #region methods
        public void OnMonthUpdate(MonthData monthData)
        {
            GenerateOffers();
        }
        [Todo("More texts variants")]
        internal void GenerateOffers()
        {
            offers.Clear();
            int sellingPlotsCount = sellingPlots.Count;
            if (sellingPlotsCount == 0) return;

            int prSkill = -1;
            IReadOnlyRole pr = GameData.Data.CompanyData.OfficeData.Divisions.PRManager;
            if (pr.IsEmployeeHired())
            {
                prSkill = pr.Employee.SkillLevel;
            }
            float maxPriceIncrease = 0;
            int minOffersCount = 0;
            int maxOffersCount = 1;
            if (prSkill > 0)
            {
                maxPriceIncrease += prSkill / 300f;
                minOffersCount += 1;
                maxOffersCount += prSkill / 20;
            }

            int newOffersCount = (int)Random.Range(minOffersCount, sellingPlotsCount * 1.3f + maxOffersCount);

            float maxSellPriceScale = 1.15f + maxPriceIncrease;
            for (int i = 0; i < newOffersCount; ++i)
            {
                SellingLandPlotData sellingPlot = sellingPlots[Random.Range(0, sellingPlotsCount)];
                int maxSellPrice = Mathf.CeilToInt(sellingPlot.Plot.MaxSellPrice * maxSellPriceScale);
                int sellingPrice = sellingPlot.SellingPrice;
                if (sellingPrice > maxSellPrice) continue;
                if (maxSellPrice < 1) continue;
                float chanceForOffer = (1.02f - sellingPrice / (float)maxSellPrice);
                float pow = 0.5f; //the less value, the higher chance
                float powChance = Mathf.Pow(chanceForOffer, pow);
                float chancePercent = powChance * 100;
                if (CustomMath.GetRandomChance(chancePercent))
                {
                    AddOffer(sellingPrice, sellingPlot, 294);
                    continue;
                }
                if (CustomMath.GetRandomChance(70 - chancePercent))
                {
                    AddOfferWithRandomPrice(sellingPrice, sellingPlot, new(0.5f, 0.8f), 295);
                    continue;
                }
                if (CustomMath.GetRandomChance(Mathf.Clamp(Mathf.Pow(powChance, 2) * 100, 10, 40)))
                {
                    AddOfferWithRandomPrice(sellingPrice, sellingPlot, new(1.15f, 1.3f), 432);
                    continue;
                }
                if (CustomMath.GetRandomChance(100 - chancePercent))
                {
                    AddOfferWithRandomPrice(sellingPrice, sellingPlot, new(0.3f, 0.5f), 431);
                    continue;
                }
            }
        }
        private void AddOfferWithRandomPrice(int sellingPrice, SellingLandPlotData sellingPlot, Vector2 randomRange, int gameTextId)
        {
            int newPrice = Mathf.RoundToInt(sellingPrice * Random.Range(randomRange.x, randomRange.y));
            newPrice = Mathf.Max(newPrice, 1);
            AddOffer(newPrice, sellingPlot, gameTextId);
        }
        private void AddOffer(int sellingPrice, SellingLandPlotData sellingPlot, int gameTextId)
        {
            LandPlotOfferData offer = new(sellingPlot, sellingPrice, gameTextId);
            offers.Add(offer);
            OnOfferAdded?.Invoke(offer);
        }
        private void RemoveOffersWithPlotId(int offerPlotId)
        {
            if (offerPlotId < 0) return;
            int offersCount = offers.Count;
            for (int i = offersCount - 1; i >= 0; --i)
            {
                LandPlotOfferData currentOffer = offers[i];
                if (currentOffer.SellingPlot.Plot.Id == offerPlotId)
                    offers.RemoveAt(i);
            }
        }
        public void ConfirmOffer(LandPlotOfferData offer)
        {
            if (offer == null) return;
            TrySellPlot(offer.SellingPlot.Plot.Id, offer.SellingPrice);
        }
        public bool IsSold(int id, out SellingLandPlotData plot)
        {
            plot = null;
            if (id < 0) return false;
            int dataCount = soldPlots.Count;
            for (int i = 0; i < dataCount; ++i)
            {
                SellingLandPlotData landPlot = soldPlots[i];
                if (landPlot.Plot.Id == id)
                {
                    plot = landPlot;
                    return true;
                }
            }
            return false;
        }
        public bool IsSelling(int id, out SellingLandPlotData plot)
        {
            plot = null;
            if (id < 0) return false;
            int dataCount = sellingPlots.Count;
            for (int i = 0; i < dataCount; ++i)
            {
                SellingLandPlotData landPlot = sellingPlots[i];
                if (landPlot.Plot.Id == id)
                {
                    plot = landPlot;
                    return true;
                }
            }
            return false;
        }
        public bool Exists(int id, out LandPlotData plot)
        {
            plot = null;
            if (id < 0) return false;
            int dataCount = plots.Count;
            for (int i = 0; i < dataCount; ++i)
            {
                LandPlotData landPlot = plots[i];
                if (landPlot.Id == id)
                {
                    plot = landPlot;
                    return true;
                }
            }
            return false;
        }
        public bool TryFinishBlueprint(int plotId)
        {
            if (!Exists(plotId, out LandPlotData plot)) return false;
            if (plot.BlueprintBaseIdReference < -1) return false;
            OnPlotBlueprintFinished?.Invoke(plot);
            return true;
        }
        public bool TryStartSelling(int id, int price)
        {
            if (price < 1) return false;
            if (IsSelling(id, out _)) return false;
            if (IsSold(id, out _)) return false;
            if (!Exists(id, out LandPlotData plot)) return false;
            GameData.Data.ConstructionsData.TryCancelBuild(plot.BlueprintBaseIdReference);
            sellingPlots.Add(new(price, plot));
            OnPlotStartedSelling?.Invoke(plot);
            return true;
        }
        public bool TryEndSelling(int id)
        {
            if (!IsSelling(id, out SellingLandPlotData sellingPlot)) return false;
            RemoveOffersWithPlotId(sellingPlot.Plot.Id);
            sellingPlots.Remove(sellingPlot);
            OnPlotEndedSelling?.Invoke(sellingPlot.Plot);
            return true;
        }
        public bool TrySellPlot(int id, int price)
        {
            if (price < 1) return false;
            if (!IsSelling(id, out SellingLandPlotData plot)) return false;
            SellPlot(plot, price);
            return true;
        }
        private void SellPlot(SellingLandPlotData sellingPlot, int price)
        {
            GameData.Data.PlayerData.Wallet.TryIncreaseValue(price);
            if (GameData.Data.BlueprintsData.TryRemoveBlueprint(sellingPlot.Plot.BlueprintBaseIdReference))
            {
                sellingPlot.Plot.ResetBlueprintReference();
            }
            RemoveOffersWithPlotId(sellingPlot.Plot.Id);
            sellingPlots.Remove(sellingPlot);
            SellingLandPlotData soldPlot = new(price, sellingPlot.Plot);
            soldPlots.Add(soldPlot);
            OnPlotSold?.Invoke(sellingPlot.Plot);
        }
        public bool TryStartBlueprint(int plotId, int blueprintBaseId)
        {
            if (!Exists(plotId, out LandPlotData plot)) return false;
            if (!plot.TryStartBlueprint(blueprintBaseId)) return false;
            OnPlotBlueprintStarted?.Invoke(plot);
            return true;
        }
        public bool CanAdd(int id)
        {
            if (id < 0) return false;
            if (Exists(id, out _)) return false;
            return true;
        }
        public bool TryAdd(int id)
        {
            if (!CanAdd(id)) return false;
            LandPlotData plot = new(id);
            plots.Add(plot);
            OnPlotAdded?.Invoke(plot);
            return true;
        }
        private int CalculateBill()
        {
            int dataCount = plots.Count;
            int result = 0;
            for (int i = 0; i < dataCount; ++i)
            {
                LandPlotData landPlot = plots[i];
                if (!landPlot.CanAddBill) continue;
                if (IsSold(landPlot.Id, out _)) continue;
                result += landPlot.BillPaymentAmount;
            }
            return result;
        }
        #endregion methods
    }
}