using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SFX
{
    public class SfxManager : MonoBehaviour
    {
        [Header("~~~~~~~~~~~ SFX ELEMENTS ~~~~~~~~~~~")]
        [SerializeField] private AudioClip gateSfx;
        [SerializeField] private AudioClip buttonClickSfx;
        
        [Header("~~~~~~~~~~~ SFX SETTINGS ~~~~~~~~~~~")]
        private AudioSource _audioSource;
        private bool _isPlayingButtonSfx = false;

        public static SfxManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlayGateSfx()
        {
            if (gateSfx != null && _audioSource != null)
                _audioSource.PlayOneShot(gateSfx);
        }

        public void PlayButtonClickSfx()
        {
            if (_isPlayingButtonSfx) return;
            _isPlayingButtonSfx = true;
            
            if (buttonClickSfx != null && _audioSource != null)
                _audioSource.PlayOneShot(buttonClickSfx);

            Invoke(nameof(ResetButtonSfxFlag), 0.2f);
        }
        
        private void ResetButtonSfxFlag()
        {
            _isPlayingButtonSfx = false;
        }
    }
}