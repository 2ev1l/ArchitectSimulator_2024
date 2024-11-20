using EditorCustom.Attributes;
using Universal.Events;

namespace Game.Animation
{
    /// <summary>
    /// Using default update for better UI
    /// </summary>
    public class RotateAroundUpdate : RotateAroundBase, IUpdateSender
    {
        #region fields & properties
        protected override float DeltaTime => UnityEngine.Time.deltaTime;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            MessageController.AddSender(this);
        }
        private void OnDisable()
        {
            MessageController.RemoveSender(this);
        }
        public void UpdateMessage()
        {
            TrySimulate();
        }
        [Button(nameof(Simulate))]
        protected override void Simulate()
        {
            base.Simulate();
        }
        #endregion methods
    }
}