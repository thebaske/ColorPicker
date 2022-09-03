using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sounders
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundLooperBase : MonoBehaviour
    {
        [SerializeField] private AudioClip toLoop;
        [Range(0f, 1f)] [SerializeField] private float clipScrub;
        [Header("Speed settings")]
        [SerializeField] private AnimationCurve startingCurve;
        [SerializeField] private AnimationCurve endingCurve;
        [SerializeField] private float playbackSpeed = 1f;
        [SerializeField] private Vector2 minMaxPlaybackSpeed;
        private float clipPlayCounter;
        [Header("Pitch settings")]
        [SerializeField] private AnimationCurve startingPitch;
        [SerializeField] private AnimationCurve endingPitch;
        [SerializeField] private float soundPitch = 1f;
        [SerializeField] float clipLength;
        private float startEndCounter;
        [SerializeField] private AudioSource aSource;

        [Button("GetSndClipLength")]
        void GetClip()
        {
            clipLength = toLoop.length;
            aSource.clip = toLoop;
        }

        void Start()
        {
            aSource.clip = toLoop;
            aSource.Play();
        }

        public void Play(float minvalue, float maxValue, float currentValue)
        {
            startEndCounter += Time.deltaTime;
            startEndCounter = Mathf.Clamp(startEndCounter, 0f, startingCurve.keys[startingCurve.length - 1].value);
            float evaluated = startingCurve.Evaluate(Utility.RemapValues(minvalue, maxValue, 0f, 1f, currentValue));
            playbackSpeed = Utility.RemapValues(0f, 1f,
                minMaxPlaybackSpeed.x, minMaxPlaybackSpeed.y, evaluated);
            //Change speed and pitch;
            //read beginning curves;
        }

        public void DePlay()
        {
            startEndCounter -= Time.deltaTime;
            startEndCounter = Mathf.Clamp(startEndCounter, 0f, endingCurve.keys[0].value);
            playbackSpeed = Utility.RemapValues(0f, startingCurve.keys[startingCurve.length - 1].value,
                minMaxPlaybackSpeed.x, minMaxPlaybackSpeed.y, startEndCounter);
            //read ending curves
            //change speed and pitch
        }
        void Update()
        {
            // aSource.time = Utility.RemapValues(0f, 1f, 0f, clipLength, clipScrub);
            aSource.time = clipPlayCounter;
            clipPlayCounter += Time.deltaTime * playbackSpeed;
            if (clipPlayCounter >= 1f)
            {
                clipPlayCounter = 0f;
            }
        }
    }

}