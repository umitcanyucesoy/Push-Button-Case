using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Hand
{
    public class HandManager : MonoBehaviour
    {
        public static HandManager Instance;

        [Header("~~~~~~~~ HAND SETTINGS ~~~~~~~~~")]
        public GameObject handPrefab;
        public Transform handContainer;
        public GameObject mainHand;
        public HandData handData;
        private readonly List<GameObject> _handLists = new List<GameObject>();
        private const int MaxHands = 5;
        
        [Header("~~~~~~~~ ANIMATION SETTINGS ~~~~~~~~~")]
        public float pressDuration = 0.5f;
        public float pressDistance = 0.25f;
        private Sequence _animationSequence;
        private bool _addToRight = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (handContainer == null)
            {
                GameObject container = new GameObject("HandContainer");
                container.transform.parent = this.transform;
                container.transform.position = mainHand.transform.position;
                handContainer = container.transform;
            }

            mainHand.transform.parent = handContainer;
            _handLists.Add(mainHand);
            InitializeAnimation();
        }
        
        private void InitializeAnimation()
        {
            _animationSequence = DOTween.Sequence()
                .SetLoops(-1)
                .AppendInterval(0f)
                .Append(handContainer.DOMoveY(handContainer.position.y - pressDistance, pressDuration / 2)
                    .SetEase(Ease.InOutSine))
                .Append(handContainer.DOMoveY(handContainer.position.y, pressDuration / 2)
                    .SetEase(Ease.InOutSine));
        }
        
        private void OnDestroy()
        {
            if (_animationSequence != null && _animationSequence.IsActive())
            {
                _animationSequence.Kill();
            }
        }
        

        /// <summary>
        /// Width gate değerine göre el ekler.
        /// width: 1 veya 2
        /// </summary>
        public void AddHands(int count)
        {
            int handsToAdd = Mathf.Min(count, MaxHands - _handLists.Count);

            for (int i = 0; i < handsToAdd; i++)
            {
                Vector3 nextPosition = GetNextHandPosition();
                
                if (nextPosition.z > handData.maxZ || nextPosition.z < handData.minZ)
                {
                    Debug.LogWarning("Z sınırına ulaşıldı. Daha fazla el eklenemiyor.");
                    break;
                }

                var newHand = Instantiate(handPrefab, nextPosition, handContainer.rotation, handContainer);
                _handLists.Add(newHand);
                
                var handAnimation = newHand.GetComponent<HandAnimationTween>();
                if (handAnimation != null)
                {
                    handAnimation.enabled = false;
                }
                
                var handMovement = newHand.GetComponent<HandMovement>();
                if (handMovement != null)
                {
                    handMovement.enabled = false;
                }

                Debug.Log($"Yeni el eklendi: Pozisyon: {newHand.transform.position}");
            }
        }

        public void RemoveHands(int count)
        {
            count = Mathf.Abs(count);
            int handsToRemove = Mathf.Min(count, _handLists.Count - 1);
            for (int i = 0; i < handsToRemove; i++)
            {
                if (_handLists.Count <= 1) { return; }

                var handToRemove = _handLists[_handLists.Count - 1];
                _handLists.RemoveAt(_handLists.Count - 1);
                DOTween.Kill(handToRemove.transform);
                
                Destroy(handToRemove);
                Debug.Log($"El kaldırıldı: Pozisyon: {handToRemove.transform.position}");
            }
        }
        
        
        /// <summary>
        /// Length gate geçildiğinde mevcut tüm ellerin önüne yeni eller ekler.
        /// Y ve Z eksenlerinde değişiklik yapmaz, sadece X ekseninde sabit bir offset ile ileri ekler.
        /// </summary>
        public void AddRowToHands()
        {
            float fixedXOffset = 0.14f; // Sabit X offset değeri
            List<GameObject> currentHands = new List<GameObject>(_handLists);
            List<GameObject> newHands = new List<GameObject>();

            foreach (var hand in currentHands)
            {
                Vector3 newHandPosition = new Vector3(hand.transform.position.x + fixedXOffset, hand.transform.position.y, hand.transform.position.z);

                // X sınırlarını kontrol edebilirsiniz (handData.minX ve handData.maxX gibi) eğer gerekiyorsa
                // Örneğin:
                // if (newHandPosition.x > handData.maxX || newHandPosition.x < handData.minX)
                // {
                //     Debug.LogWarning("X sınırına ulaşıldı. Daha fazla el eklenemiyor.");
                //     continue;
                // }

                var newHand = Instantiate(handPrefab, newHandPosition, handContainer.rotation, handContainer);
                newHands.Add(newHand);
                
                var handAnimation = newHand.GetComponent<HandAnimationTween>();
                if (handAnimation != null)
                {
                    handAnimation.enabled = false;
                }

                Debug.Log($"Yeni sıra el eklendi: Pozisyon: {newHandPosition}");
            }

            _handLists.AddRange(newHands);
        }

        private Vector3 GetNextHandPosition()
        {
            float fixedZOffset = 0.2f; // Sabit Z offset değeri
            float direction = _addToRight ? 1f : -1f;
            _addToRight = !_addToRight;

            float newZ = mainHand.transform.position.z + direction * fixedZOffset;

            newZ = Mathf.Clamp(newZ, handData.minZ, handData.maxZ);

            return new Vector3(mainHand.transform.position.x, mainHand.transform.position.y, newZ);
        }
    }
}
