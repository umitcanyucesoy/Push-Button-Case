using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace KeyboardButton
{
    public class ButtonPress : MonoBehaviour
    {
        [Header("Button Settings")]
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material orangeMaterial;
        [SerializeField] private TextMeshPro buttonText;
        private int _pressCount = 0;
        private bool _isInteractable = true;

        public async UniTask AnimateButton()
        {
            if (_pressCount >= 3 || !_isInteractable) return;
            
            _pressCount++;
            _isInteractable = false;
            
            if (_pressCount == 1 && buttonText != null)
                buttonText.gameObject.SetActive(true);
            
            UpdateButtonAppearance();

            float downDistance = _pressCount == 1 ? 0.2f : 0.1f;
            float upDistance = _pressCount == 1 ? 0.25f : 0.1f;

            transform.DOMoveY(transform.position.y - downDistance, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    transform.DOMoveY(transform.position.y + upDistance, 0.1f).SetEase(Ease.OutBounce);
                });

           await UniTask.Delay(TimeSpan.FromSeconds(0.35f));
           _isInteractable = true;
        }

        private void UpdateButtonAppearance()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer == null)
                return;

            switch (_pressCount)
            {
                case 1:
                    renderer.material = greenMaterial;
                    buttonText.text = "1";
                    break;
                case 2:
                    renderer.material = blueMaterial;
                    buttonText.text = "2";
                    break;
                case 3:
                    renderer.material = orangeMaterial;
                    buttonText.text = "3";
                    break;
            }
        }
}
}
