using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("~~~~~~~~~~ UI Elements ~~~~~~~~~~")]
        [SerializeField] private RectTransform tapToPlayText;
        [SerializeField] private float scaleDuration = .5f;
        [SerializeField] private float scaleAmount = 1.1f;

        private bool _gameStarted = false;

        private void Start()
        {
            Time.timeScale = 0f;
            AnimateTapToPlay();
        }

        private void Update()
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && !_gameStarted)
                StartGame();
        }

        private void AnimateTapToPlay()
        {
            tapToPlayText.DOScale(scaleAmount, scaleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
        }

        private void StartGame()
        {
            _gameStarted = true;
           
            tapToPlayText.DOKill();
            tapToPlayText.gameObject.SetActive(false);
            
            Time.timeScale = 1f; 
        }
    }
}