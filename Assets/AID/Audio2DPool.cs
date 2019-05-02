using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace AID
{
    /*
        Helper class for keeping a collection of audio sources that need to be reused
    */
    public class Audio2DPool : MonoBehaviour
    {
        private List<AudioSource> sources = new List<AudioSource>();
        public AudioMixerGroup mixerGroup;

        public AudioSource GetTempSource()
        {
            AudioSource ret = null;

            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];

                if (!s.isPlaying || s.clip == null || s.timeSamples >= s.clip.samples)
                {
                    ret = sources[i];
                    break;
                }
            }

            if (ret == null)
            {
                sources.Add(ret = gameObject.AddComponent<AudioSource>());
            }

            ret.spatialize = false;
            ret.volume = 1;
            ret.pitch = 1;
            ret.outputAudioMixerGroup = mixerGroup;
            ret.loop = false;
            return ret;
        }

        public void StopAll()
        {
            for (int i = 0; i < sources.Count; i++)
            {
                sources[i].Stop();
            }
        }
    }
}