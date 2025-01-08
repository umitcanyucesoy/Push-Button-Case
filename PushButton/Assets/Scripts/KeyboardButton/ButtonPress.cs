using DG.Tweening;
using UnityEngine;

namespace KeyboardButton
{
    public class ButtonPress : MonoBehaviour
    {
        [Header("Button Settings")]
        [SerializeField] private Material pressedMaterial; 
        private bool _isPressed = false; 

        public void AnimateButton()
        {
            if (_isPressed) return;
            _isPressed = true;

            transform.DOMoveY(transform.position.y - 0.2f, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    transform.DOMoveY(transform.position.y + 0.25f, 0.1f).SetEase(Ease.OutBounce);

                    GetComponent<Renderer>().material = pressedMaterial;
                });
        }
    }
}