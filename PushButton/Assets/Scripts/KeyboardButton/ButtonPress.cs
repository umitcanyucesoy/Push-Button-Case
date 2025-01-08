using DG.Tweening;
using UnityEngine;

namespace KeyboardButton
{
    public class ButtonPress : MonoBehaviour
    {
        [Header("Button Settings")]
        public Material defaultMaterial; 
        public Material pressedMaterial; 
        private bool isPressed = false; 

        public void AnimateButton()
        {
            if (isPressed) return; // Zaten basıldıysa işlemi tekrar etme
            isPressed = true;

            transform.DOMoveY(transform.position.y - 0.2f, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    transform.DOMoveY(transform.position.y + 0.25f, 0.1f).SetEase(Ease.OutBounce);

                    GetComponent<Renderer>().material = pressedMaterial;
                });
        }
    }
}