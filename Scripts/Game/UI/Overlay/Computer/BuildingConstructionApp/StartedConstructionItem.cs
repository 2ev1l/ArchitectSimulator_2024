using Game.Serialization.World;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class StartedConstructionItem : ConstructionDataItem
    {
        #region fields & properties
        [SerializeField] private CustomButton buttonComplete;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            buttonComplete.OnClicked += CompleteBuild;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            buttonComplete.OnClicked -= CompleteBuild;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            bool btnState = Context.BuildCompletionMonth <= GameData.Data.PlayerData.MonthData.CurrentMonth;
            GameObject buttonObj = buttonComplete.gameObject;
            if (buttonObj.activeSelf != btnState)
            {
                buttonObj.SetActive(btnState);
            }
        }
        private void CompleteBuild()
        {
            //store base id because 'onbeforebuildcompleted' may cause list update with wrong id
            int baseId = Context.BaseId;
            OnBeforeBuildCompleted();
            GameData.Data.ConstructionsData.TryBuildByBaseId(baseId);
        }
        protected virtual void OnBeforeBuildCompleted() { }
        #endregion methods
    }
}