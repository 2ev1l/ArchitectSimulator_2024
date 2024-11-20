using Game.Audio;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Time;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class MusicObserver : Observer
    {
        #region fields & properties
        [SerializeField] private List<AudioClip> gameStartLocationMusic;
        [SerializeField] private List<AudioClip> gameWorkLocationMusic;
        private readonly Timer MusicTimer = new();
        #endregion fields & properties

        #region methods
        public override void Initialize()
        {
            GameData.Data.LocationsData.OnLocationChanged += PlayMusicDependingOnLocation;
            MusicTimer.OnChangeEnd = PlayMusicDependingOnLocation;
            PlayMusicDependingOnLocation();
        }
        public override void Dispose()
        {
            GameData.Data.LocationsData.OnLocationChanged -= PlayMusicDependingOnLocation;
        }
        private void RestartMusicTimer()
        {
            float length = AudioManager.CurrentQueueClip.length - 1;
            length = Mathf.Max(0.1f, length);
            MusicTimer.Restart(length);
        }
        private void PlayMusicDependingOnLocation() => PlayMusicDependingOnLocation(GameData.Data.LocationsData.CurrentLocationId);
        private void PlayMusicDependingOnLocation(int locationId)
        {
            AudioClip clip = (locationId) switch
            {
                0 => GetRandomClip(gameStartLocationMusic),
                1 => GetRandomClip(gameWorkLocationMusic),
                _ => GetRandomClip(gameStartLocationMusic)
            };

            AudioManager.SwitchMusic(clip, 2f);
            RestartMusicTimer();
        }
        private AudioClip GetRandomClip(List<AudioClip> clips) => clips[Random.Range(0, clips.Count)];
        #endregion methods

    }
}