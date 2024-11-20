using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Overlay
{
    public class DescriptionTaskPanel<TaskData, Info> : DescriptionPanel<TaskData> where TaskData : TaskData<Info> where Info : TaskInfo
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI nameText;
        protected TextMeshProUGUI DescriptionText => descriptionText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        protected GameObject DescriptionGroup => descriptionGroup;
        [SerializeField] private GameObject descriptionGroup;
        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private GameObject rewardGroup;

        [SerializeField] private TextMeshProUGUI timeLeftText;
        #endregion fields & properties

        #region methods
        protected override void OnDataNull()
        {
            base.OnDataNull();
            if (ActiveItem != null)
            {
                ActiveItem.SetItemActive(false);
            }
        }

        protected override void OnUpdateUI()
        {
            if (ActiveItem != null && (ActiveItem.Context != Data || !ActiveItem.isActiveAndEnabled))
            {
                OnDataNull();
                return;
            }

            nameText.text = Data.Info.NameInfo.Text;
            descriptionText.text = Data.Info.DescriptionInfo.Text;
            descriptionGroup.SetActive(!descriptionText.text.Equals(""));
            rewardText.text = Data.Info.RewardInfo.GetLanguage();
            rewardGroup.SetActive(Data.Info.RewardInfo.Rewards.Count() != 0);

            int timeLeft = Data.MonthLeft;
            if (timeLeft >= 0)
                timeLeftText.text = $"{timeLeft} m.";
        }
        #endregion methods

    }
}