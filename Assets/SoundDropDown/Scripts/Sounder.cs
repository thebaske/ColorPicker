using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DataSystem;

namespace Sounders
{
    public class Sounder : MonoBehaviour
    {
        [SerializeField] UnityEvent OnSoundPlayed;
        public List<SoundIndexerHolder> MySounds = new List<SoundIndexerHolder>();
        GameEvent OnSoundRequested;
        List<int> rndmizer = new List<int>();
        public bool playOnEnable;
        public bool playOnDisable;
        SounderMixer mixer;
        public static Sounder sd { get; private set; }

        private void Awake()
        {
            sd = this;
            mixer = FindObjectOfType<SounderMixer>();
        }



        private void OnEnable()
        {
            if (playOnEnable)
            {
                PlaySoundHandler(GetRandomIndexOfThisName("OnEnable"));
            }
        }

        private void OnDisable()
        {
            if (playOnDisable)
            {
                PlaySoundHandler(GetRandomIndexOfThisName("OnDisable"));
            }
        }

        public void PlayRandom()
        {
            PlaySoundHandler(GetRandomIndexOfThisName("OnDefault"));
        }

        public void PlayDefault()
        {
            PlaySoundHandler(GetRandomIndexOfThisName("OnDefault"));
        }

        public void PlaySoundHandler(int id)
        {
            if (mixer != null)
            {
                mixer.PlaySound(id);
            }
            else
            {
                mixer = FindObjectOfType<SounderMixer>();
                if (mixer != null)
                {
                    mixer.PlaySound(id);
                }
            }

            OnSoundPlayed?.Invoke();
        }

        public void PlaySoundHandler(string criteria)
        {
            if (mixer != null)
            {
                mixer.PlaySound(GetRandomIndexOfThisName(criteria));
            }

            OnSoundPlayed?.Invoke();
        }

        public void PlaySoundHandlerVolume(string criteria)
        {
            if (mixer != null)
            {
                SoundData randIndex = GetRandomIndexOfThisNameVolume(criteria);
                
                mixer.PlaySoundVolume(randIndex.index, randIndex.volume);
            }

            OnSoundPlayed?.Invoke();
        }
        
        private int GetRandomIndexOfThisName(string namer)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < MySounds.Count; i++)
            {
                if (MySounds[i].Name == namer)
                {
                    indexes.Add(MySounds[i].index);
                }
            }

            return indexes[Random.Range(0, indexes.Count)];
        }
        private SoundData GetRandomIndexOfThisNameVolume(string namer)
        {
            List<int> indexes = new List<int>();
            List<SoundData> snds = new List<SoundData>();
            for (int i = 0; i < MySounds.Count; i++)
            {
                if (MySounds[i].Name == namer)
                {
                    indexes.Add(MySounds[i].index);
                    snds.Add(new SoundData(MySounds[i].index, MySounds[i].volume));
                }
            }

            return snds[Random.Range(0, snds.Count)];
        }

        private struct SoundData
        {
            public int index;
            public float volume;

            public SoundData(int indx, float vlm)
            {
                index = indx;
                volume = vlm;
            }
        }
        public List<int> GetIndexOfThisName(string namer)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < MySounds.Count; i++)
            {
                if (MySounds[i].Name == namer)
                {
                    indexes.Add(MySounds[i].index);
                }
            }

            return indexes;
        }
    }
}
