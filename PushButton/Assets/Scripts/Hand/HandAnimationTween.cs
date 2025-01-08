using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Hand
{
    public class HandAnimationTween : MonoBehaviour
    {
        [Header("~~~~~~~~~ Animation Settings ~~~~~~~~~")]
        [SerializeField] private float pressDuration = 0.5f; 
        [SerializeField] private float pressDistance = 0.5f; 
        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
            HandPressAnimation().Forget();
        }

        private async UniTask HandPressAnimation()
        {
            while (true)
            {
                transform.DOMoveY(_startPosition.y - pressDistance, pressDuration / 2).SetEase(Ease.InOutSine);
                await UniTask.Delay(TimeSpan.FromSeconds(pressDuration/2));
                
                transform.DOMoveY(_startPosition.y, pressDuration / 2).SetEase(Ease.InOutSine);
                await UniTask.Delay(TimeSpan.FromSeconds(pressDuration/2));
            }
        }
    }
}