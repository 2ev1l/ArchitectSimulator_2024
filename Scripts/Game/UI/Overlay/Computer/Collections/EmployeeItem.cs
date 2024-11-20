using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class EmployeeItem : ContextActionsItem<EmployeeData>
    {
        #region fields & properties
        [SerializeField] private HumanItem humanItem;
        [SerializeField] private TextMeshProUGUI roleText;
        [SerializeField] private TextMeshProUGUI salaryText;
        [SerializeField] private Slider skillSlider;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            roleText.text = Context.ToLanguage();
            salaryText.text = $"${Context.Salary} / m.";
            skillSlider.value = Context.SkillLevel;
        }
        public override void OnListUpdate(EmployeeData param)
        {
            base.OnListUpdate(param);
            humanItem.OnListUpdate(param.HumanInfo);
        }
        #endregion methods
    }
}