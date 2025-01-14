using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.DataBase
{
    [System.Serializable]
    public class RewardInfo : ICloneable<RewardInfo>
    {
        #region fields & properties
        public UnityEvent OnRewardAdded;
        public IReadOnlyList<Reward> Rewards => rewards;
        [SerializeField] private Reward[] rewards;
        public RewardRequest RewardRequest
        {
            get
            {
                rewardRequest ??= new(this);
                return rewardRequest;
            }
        }
        [System.NonSerialized] private RewardRequest rewardRequest = null;
        #endregion fields & properties

        #region methods
        public bool HasReward(RewardType type, out Reward reward) => rewards.Exists(x => x.Type == type, out reward);
        public void AddReward()
        {
            RewardRequest.Send();
        }
        public string GetLanguage()
        {
            string result = "";
            foreach (var el in rewards)
                result += $"{el.GetLanguage()}\n";
            return result;
        }
        public RewardInfo(params Reward[] rewards)
        {
            this.rewards = rewards;
        }
        public RewardInfo Clone() => new(rewards.ToArray());
        #endregion methods
    }
}