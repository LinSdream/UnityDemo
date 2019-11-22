using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;

namespace Game
{
    public class SoundManager : ASingletonBasis<SoundManager>
    {
        #region Fields
        public AudioSource EfxSource;
        public AudioSource MusicSource;
        public float LowPitchRange = 0.95f;
        public float HighPitchRange = 1.05f;

        #endregion

        #region Public Methods
        public void PlaySingle(AudioClip clip)
        {
            EfxSource.clip = clip;
            EfxSource.Play();
        }

        public void RandomPitchSFX(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

            EfxSource.clip = clips[randomIndex];
            EfxSource.pitch = randomPitch;

            EfxSource.Play();
        }

        #endregion

        #region Override Methods
        #endregion
    }

}