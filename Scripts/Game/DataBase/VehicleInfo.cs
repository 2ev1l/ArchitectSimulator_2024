using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class VehicleInfo : DBInfo, IPreviewHandler, IMoodScaleHandler
    {
        #region fields & properties
        public GameObject HousePrefab => housePrefab;
        [SerializeField] private GameObject housePrefab;
        public GameObject CityPrefab => cityPrefab;
        [SerializeField] private GameObject cityPrefab;
        public Sprite PreviewSprite => previewSprite;
        [SerializeField] private Sprite previewSprite;
        public string Name => name;
        [SerializeField] private string name;
        public float MoodScale => moodScale;
        [SerializeField][Min(0.1f)] private float moodScale = 1;

        public int MaxSpeed => maxSpeed;
        [Title("UI")][SerializeField][Min(20)] private int maxSpeed = 60;
        public float Time0_100 => time0_100;
        [SerializeField][Range(0.2f, 100)] private float time0_100 = 10f;
        public int ReleaseYear => releaseYear;
        [SerializeField][Min(1900)] private int releaseYear = 2000;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}