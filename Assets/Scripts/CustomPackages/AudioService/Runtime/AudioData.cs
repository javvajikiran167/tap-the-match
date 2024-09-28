using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common.Audio
{
    [CreateAssetMenu(fileName = "AudioDB", menuName = "ScriptableObjects/AudioDB", order = 1)]
    public class AudioData : ScriptableObject
    {
        [SerializeField]private List<AudioFile> audioFiles;

        private Dictionary<string, AudioFile> audioDictionary = new Dictionary<string, AudioFile>();

        public void Init()
        {
            audioDictionary.Clear();
            foreach (var audio in audioFiles)
            {
                audioDictionary.Add(audio.name, audio);
            }
        }

        public AudioFile GetAudio(string audioName)
        {
            if (audioDictionary.ContainsKey(audioName))
            {
                return audioDictionary[audioName];
            }
            else
            {
                return null;
            }
        }


    }

    [System.Serializable]
    public class AudioFile
    {
        public string name;
        public AudioClip audioClip;
        public float volume = 1;
        public bool isLoop;
    }
}