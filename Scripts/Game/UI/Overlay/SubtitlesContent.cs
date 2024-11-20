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
    public class SubtitlesContent : DestroyablePoolableObject
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private ValueTimeChanger textChanger = new();
        private string finalText = "";
        [Title("Animation")]
        [SerializeField] private ObjectScale scaleAnimator;
        #endregion fields & properties

        #region methods
        private void ChangeText()
        {
            textChanger.SetValues(0, 1);
            textChanger.SetActions(x =>
            {
                int textRange = (int)Mathf.Lerp(0, finalText.Length, x);
                subtitleText.text = finalText[..textRange];
            },
            delegate { subtitleText.text = finalText; });
            float speed = TextData.LoadedData.AverageCharacterReadingPerSecond * 2 * SettingsData.Data.LanguageSettings.SubtitleSpeed;
            textChanger.Restart(finalText.Length / speed);
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
            LiveTime = TextData.LoadedData.GetAverageTextReadTime(finalText);
            ChangeText();
        }
        #endregion methods
    }
}