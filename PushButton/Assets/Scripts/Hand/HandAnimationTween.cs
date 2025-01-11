using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Hand
{
    public class HandAnimationTween : MonoBehaviour
    {
        [Header("~~~~~~~~~ Animation Settings ~~~~~~~~~")]
        [SerializeField] private float pressDuration = 0.2f; 
        [SerializeField] private float pressDistance = 0.25f; 
        private Vector3 _startPosition;
        private bool _isDestroyed = false;

        private void Start()
        {
            _startPosition = transform.position;
            HandPressAnimation().Forget();
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        private async UniTask HandPressAnimation()
        {
            while (true)
            {
                if (_isDestroyed) break;

                transform.DOMoveY(_startPosition.y - pressDistance, pressDuration / 2).SetEase(Ease.InOutSine);
                await UniTask.Delay(TimeSpan.FromSeconds(pressDuration / 2));

                if (_isDestroyed) break;

                transform.DOMoveY(_startPosition.y, pressDuration / 2).SetEase(Ease.InOutSine);
                await UniTask.Delay(TimeSpan.FromSeconds(pressDuration / 2));
            }
        }
    }
}