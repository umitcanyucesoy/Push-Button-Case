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
        
        // Animasyon ayarları
        [Header("~~~~~~~~ ANIMATION SETTINGS ~~~~~~~~~")]
        public float pressDuration = 0.5f;
        public float pressDistance = 0.25f;
        private Sequence _animationSequence;

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
            _handLists.Add(mainHand);
            InitializeAnimation();
        }
        
        private void InitializeAnimation()
        {
            _animationSequence = DOTween.Sequence()
                .SetLoops(-1) // Sonsuz döngü
                .AppendInterval(0f) // Başlangıçta herhangi bir gecikme olmaz
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

        public void AddHands(int count)
        {
            int handsToAdd = Mathf.Min(count, MaxHands - _handLists.Count);

            for (int i = 0; i < handsToAdd; i++)
            {
                var newHand = Instantiate(handPrefab, GetNextHandPosition(), handContainer.rotation, handContainer);
                _handLists.Add(newHand);
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
            }
        }
        
        public void AddRowToHands()
        {
            List<GameObject> newHands = new List<GameObject>();

            foreach (var hand in _handLists)
            {
                // Yeni elin pozisyonunu hesapla
                Vector3 newHandPosition = new Vector3(
                    hand.transform.position.x + 0.5f,  // X ekseninde öne
                    hand.transform.position.y,        // Y ekseni sabit
                    hand.transform.position.z         // Z ekseni sabit
                );

                GameObject newHand = Instantiate(handPrefab, newHandPosition, handContainer.rotation, handContainer);
                newHands.Add(newHand);

                // Yeni ele animasyon ekle
                var newHandAnimation = newHand.GetComponent<HandAnimationTween>();
                if (newHandAnimation != null)
                {
                    newHandAnimation.enabled = true;
                }
            }

            _handLists.AddRange(newHands);
        }
        
        public bool IsAtBoundary(float moveAmount)
        {
            foreach (var hand in _handLists)
            {
                float newZPosition = hand.transform.position.z + moveAmount;
                
                if (newZPosition <= handData.minZ || newZPosition >= handData.maxZ)
                    return true;
            }

            return false;
        }

        private Vector3 GetNextHandPosition()
        {
            float offset = 0.15f;
            Vector3 basePosition = _handLists[0].transform.position;

            int handIndex = _handLists.Count;
            float direction = handIndex % 2 == 0 ? 1f : -1f;
            
            float zPosition = basePosition.z + (direction * offset * ((handIndex + 1) / 2));
            return new Vector3(basePosition.x, basePosition.y, zPosition);
        }
    }
}