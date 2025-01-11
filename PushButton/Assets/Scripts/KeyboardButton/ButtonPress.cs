using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SFX;
using TMPro;
using UnityEngine;

namespace KeyboardButton
{
    public class ButtonPress : MonoBehaviour
    {
        [Header("~~~~~~~~~~ Button ELEMENTS ~~~~~~~~~~")]
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material orangeMaterial;
        [SerializeField] private TextMeshPro buttonText;
        private int _pressCount = 0;
        private bool _isAnimating = false;

        public async UniTask AnimateButton()
        {
            if (_pressCount >= 3 || _isAnimating) return;

            _pressCount++;
            if (_pressCount == 1 && buttonText != null)
                buttonText.gameObject.SetActive(true);
            
            UpdateButtonAppearance();

            _isAnimating = true;

            float downDistance = _pressCount == 1 ? 0.2f : 0.1f;
            float upDistance = _pressCount == 1 ? 0.25f : 0.1f;

            await transform.DOMoveY(transform.position.y - downDistance, 0.1f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
            await transform.DOMoveY(transform.position.y + upDistance, 0.1f).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
            SfxManager.Instance.PlayButtonClickSfx();
            _isAnimating = false;
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
