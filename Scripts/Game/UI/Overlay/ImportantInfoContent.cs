using EditorCustom.Attributes;
using Game.UI.Text;
using TMPro;
using UnityEngine;
using Universal.Time;
using Game.Animation;
using Universal.Collections;
using Game.DataBase;
using Game.Serialization.World;
using Game.Serialization.Settings;

namespace Game.UI.Overlay
{
    public class ImportantInfoContent : DestroyablePoolableObject
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI text;
        private string finalText = "";
        [Title("Animation")]
        [SerializeField] private ObjectScale scaleAnimator;
        #endregion fields & properties

        #region methods
        private void ChangeText()
        {
            text.text = finalText;
            transform.localScale = new Vector3(1, 0, 1);
            IncreaseScale();
            Invoke(nameof(DecreaseScale), LiveTime - scaleAnimator.ScaleTime);
        }
        private void DecreaseScale()
        {
            scaleAnimator.ScaleTo(new Vector3(1, 0, 1));
        }
        private void IncreaseScale()
        {
            scaleAnimator.ScaleTo(new Vector3(1, 1, 1));
        }
        public void UpdateUI(string text)
        {
            CancelInvoke(nameof(DecreaseScale));
            finalText = text;
            LiveTime = TextData.LoadedData.GetAverageTextReadTime(finalText) * 0.8f;
            ChangeText();
        }
        #endregion methods
    }
}