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
        
        private float _currentZOffset = 0f;
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
                Vector3 newHandPosition = new Vector3(hand.transform.position.x + 0.3f, hand.transform.position.y, hand.transform.position.z);
                
                if (newHandPosition.z > handData.maxZ || newHandPosition.z < handData.minZ)
                {
                    Debug.LogWarning("Z sınırına ulaşıldı. Daha fazla el eklenemiyor.");
                    break;
                }

                GameObject newHand = Instantiate(handPrefab, newHandPosition, handContainer.rotation, handContainer);
                newHands.Add(newHand);
                
                var handAnimation = newHand.GetComponent<HandAnimationTween>();
                if (handAnimation != null)
                {
                    handAnimation.enabled = false;
                }
            }

            _handLists.AddRange(newHands);
        }

        private Vector3 GetNextHandPosition()
        {
            float spacing = .1f;
            _currentZOffset += spacing;
            
            float direction = _addToRight ? 1f : -1f;
            _addToRight = !_addToRight;

            float newZ = _handLists[0].transform.position.z + direction * _currentZOffset;
            
            newZ = Mathf.Clamp(newZ, handData.minZ, handData.maxZ);
            
            Vector3 basePosition = _handLists[0].transform.position;
            return new Vector3(basePosition.x, basePosition.y, newZ);
        }
    }
}
