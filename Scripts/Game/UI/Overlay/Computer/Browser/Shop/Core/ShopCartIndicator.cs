using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ShopCartIndicator<T> : MonoBehaviour where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI counter;
        [SerializeField] private GameObject indicator;
        [SerializeField] private VirtualShopCartBehaviour<T> shop;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            shop.CartData.OnItemAdded += UpdateUI;
            shop.CartData.OnItemRemoved += UpdateUI;
            UpdateUI();
        }
        private void OnDisable()
        {
            shop.CartData.OnItemAdded -= UpdateUI;
            shop.CartData.OnItemRemoved -= UpdateUI;
        }
        private void UpdateUI(CountableItem<T> _) => UpdateUI();
        protected virtual void UpdateUI()
        {
            int count = shop.CartData.Items.Count;
            counter.text = count.ToString();
            indicator.SetActive(count > 0);
        }
        #endregion methods
    }
}