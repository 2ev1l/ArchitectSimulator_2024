using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Universal.Core;
using Universal.Serialization;
using Zenject;

namespace Game.DataBase
{
    public class TextData : MonoBehaviour
    {
        #region fields & properties
        public LanguageData EnglishLanguageData => languageData;
        [SerializeField] private LanguageData languageData;
        public static LanguageData LoadedData
        {
            get => loadedData;
            set => loadedData = value;
        }
        private static LanguageData loadedData;
        #endregion fields & properties

        #region methods
        #endregion methods

#if UNITY_EDITOR
        [Button(nameof(SaveLanguage))]
        private void SaveLanguage()
        {
            LanguageData data = languageData;
            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(LanguageData.LanguagePath, $"English.json");
            File.WriteAllText(path, json);
            Debug.Log(path + " saved");
        }

        [Button(nameof(ExportToTxt))]
        private void ExportToTxt()
        {
            LanguageData data = LoadedData;
            string path = Application.persistentDataPath + "/export.txt";
            string text = "";

            File.WriteAllText(path, text);
        }
        [Title("Debug")]
        [SerializeField] private string textToFind;
        [SerializeField] private bool debugTextOnValidate;
        [Button(nameof(FindTextEntry))]
        private void FindTextEntry()
        {
            DebugTextEntriesIn(TextType.Menu, languageData.MenuData);
            DebugTextEntriesIn(TextType.Game, languageData.GameData);
            DebugTextEntriesIn(TextType.Task, languageData.TasksData);
            DebugTextEntriesIn(TextType.Resource, languageData.ResourcesData);
            DebugTextEntriesIn(TextType.Subtitle, languageData.SubtitlesData);
        }
        private void DebugTextEntriesIn(TextType type, string[] array)
        {
            FindSameTextIn(array, out List<int> found);
            foreach (int id in found)
            {
                Debug.Log($"<color=#BBFFBB>#{id}</color> - {type} == {array[id]}");
            }
        }
        private void FindSameTextIn(string[] array, out List<int> found)
        {
            found = new();
            string searchText = textToFind;
            for (int i = 0; i < array.Length; ++i)
            {
                if (searchText.Contains(array[i], System.StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(i);
                    continue;
                }
                if (array[i].Contains(searchText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    found.Add(i);
                    continue;
                }
            }
        }
        private string GetTextFromArray(string[] array)
        {
            string result = "";
            foreach (var el in array)
            {
                result += $"{LanguageInfo.RemoveXMLTags(el)}\n";
            }

            return result;
        }
        private void OnValidate()
        {
            if (!debugTextOnValidate) return;
            CheckMultipleArrayEntries(languageData.MenuData, "Menu");
            CheckMultipleArrayEntries(languageData.GameData, "Game");
            CheckMultipleArrayEntries(languageData.TasksData, "Tasks");
            CheckMultipleArrayEntries(languageData.SubtitlesData, "Subtitles");
            CheckMultipleArrayEntries(languageData.ResourcesData, "Resources");
        }
        private void CheckMultipleArrayEntries(string[] array, string name)
        {
            HashSet<string> equals = array.FindEquals((x, y) => x.Equals(y));
            foreach (var el in equals)
            {
                if (el.Equals("")) continue;
                Debug.LogError($"Multiple text '{el}' in {name}");
            }
        }

#endif //UNITY_EDITOR
    }
}