using Custom.Utils;
using UnityEngine;

namespace Common.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        [SerializeField] private AudioData audioData;

        [SerializeField] private AudioSource music, sfx,loopSfx;

        public static AudioManager Instance
        {
            get {
                return instance;
            }
        }

        private void Awake()
        {
            audioData.Init();
            instance = this;
        }

        public void PlayMusic(string audioName)
        {
            var audioFile = audioData.GetAudio(audioName);
            if (audioFile != null)
            {
                music.clip = audioFile.audioClip;
                music.volume = audioFile.volume;
                music.Play();
            }
            else
            {
                LogUtils.LogError("Could not play the audio, " + audioName + " is does not exists");
            }
        }

        public void PlaySfx(string audioName)
        {
            if (audioData == null)
            {
                LogUtils.LogError("Audio data is null");
            }
            var audioFile = audioData.GetAudio(audioName);
            if (audioFile != null)
            {
                if (audioFile.isLoop)
                {
                    if (loopSfx != null)
                    {
                        loopSfx.volume = audioFile.volume;
                        loopSfx.Stop();
                        loopSfx.clip = audioFile.audioClip;
                        loopSfx.Play();
                    }
                    else
                    {
                        LogUtils.LogError("Could not find loopSfx audio source");
                    }
                }else
                {
                    if (sfx != null)
                    {
                        sfx.volume = audioFile.volume;
                        sfx.PlayOneShot(audioFile.audioClip);
                    }
                    else
                    {
                        LogUtils.LogError("Could not find sfx audio source");
                    }
                }
                
            }
            else
            {
                LogUtils.LogError("Could not play the audio, " + audioName + " is does not exists");
            }
        }

        public void PlaySfx(string audioName, float startTime, float endTime)
        {
            if (audioData == null)
            {
                LogUtils.LogError("Audio data is null");
            }

            var audioFile = audioData.GetAudio(audioName);
            if (audioFile != null)
            {
                sfx.volume = audioFile.volume;
                sfx.PlayOneShot(MakeSubclip(audioFile.audioClip, startTime, endTime));
            }
            else
            {
                LogUtils.LogError("Could not play the audio, " + audioName + " is does not exists");
            }
        }

        /// <summary>
        /// Play last n seconds from the specified audio
        /// </summary>
        /// <param name="audioName"></param>
        /// <param name="seconds"></param>
        public void PlaySfxFromLast(string audioName,float seconds)
        {
            var audioFile = audioData.GetAudio(audioName);
            if (audioFile != null)
            {
                if(audioFile.audioClip == null)
                {
                    LogUtils.LogError("Audio clip is null");
                }
                float start = audioFile.audioClip.length > seconds ? audioFile.audioClip.length - seconds : 0;
                sfx.volume = audioFile.volume;
                sfx.clip = audioFile.audioClip;
                sfx.time = start;
                sfx.Play();
            }
            else
            {
                LogUtils.LogError("Could not play the audio, " + audioName + " is does not exists");
            }
        }

        private AudioClip MakeSubclip(AudioClip clip, float startTime, float stopTime)
        {
            int frequency = clip.frequency;
            float timeLength = stopTime - startTime;
            int samplesLength = (int)(frequency * timeLength);
            AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
            float[] data = new float[samplesLength];
            clip.GetData(data, (int)(frequency * startTime));
            newClip.SetData(data, 0);
            return newClip;
        }

        public void StopSfx()
        {
            loopSfx.Stop();
        }

        public void StopMusic()
        {
            music.Stop();
        }

        public void MuteOrUnmuteSfx(bool isMute)
        {
            sfx.mute = isMute;
            loopSfx.mute = isMute;
        }
    }
}
