using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Hand
{
    public class HandManager : MonoBehaviour
    {
        [Header("~~~~~~~~ HAND ELEMENTS ~~~~~~~~~")]
        [SerializeField] private GameObject handPrefab; 
        [SerializeField] private GameObject mainHand;
        [SerializeField] private HandData handData;
        public Transform handContainer;
        
        [Header("~~~~~~~~ HAND SETTINGS ~~~~~~~~")]
        private readonly List<GameObject> _handLists = new List<GameObject>();
        private const int MaxHands = 5;
        
        [Header("~~~~~~~~ ANIMATION SETTINGS ~~~~~~~~~")]
        public float pressDuration = 0.5f;
        private Sequence _animationSequence;
        private bool _addToRight = true;
        private float _initialYPosition;
        private const float FixedMovementRange = 0.25f;
        
        public static HandManager Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
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
            _initialYPosition = handContainer.position.y;
            InitializeAnimation();
        }
        
        private void InitializeAnimation()
        {
            _animationSequence = DOTween.Sequence()
                .SetLoops(-1)
                .AppendInterval(0f)
                .Append(handContainer.DOMoveY(_initialYPosition - FixedMovementRange, pressDuration / 2)
                    .SetEase(Ease.InOutQuad))
                .Append(handContainer.DOMoveY(_initialYPosition, pressDuration / 2)
                    .SetEase(Ease.InOutQuad));
        }
        
        private void OnDestroy()
        {
            if (_animationSequence != null && _animationSequence.IsActive())
                _animationSequence.Kill();
        }
        
        public void AddHands(int count)
        {
            int handsToAdd = Mathf.Min(count, MaxHands - _handLists.Count);

            for (int i = 0; i < handsToAdd; i++)
            {
                Vector3 nextPosition = GetNextHandPosition();
                
                if (nextPosition.z > handData.maxZ || nextPosition.z < handData.minZ) {break;}

                var newHand = Instantiate(handPrefab, nextPosition, handContainer.rotation, handContainer);
                _handLists.Add(newHand);
                
                var handMovement = newHand.GetComponent<HandMovement>();
                if (handMovement)
                    handMovement.enabled = false;
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
            float fixedXOffset = 0.14f;
            List<GameObject> currentHands = new List<GameObject>(_handLists);
            List<GameObject> newHands = new List<GameObject>();

            foreach (var hand in currentHands)
            {
                Vector3 newHandPosition = new Vector3(hand.transform.position.x + fixedXOffset, hand.transform.position.y, hand.transform.position.z);

                var newHand = Instantiate(handPrefab, newHandPosition, handContainer.rotation, handContainer);
                newHands.Add(newHand);
            }

            _handLists.AddRange(newHands);
        }
        
        public void UpdatePressDuration(float valueChange)
        {
            pressDuration = Mathf.Clamp(pressDuration + valueChange, 0.35f, .7f); 
            RestartAnimation();
        }
        
        private void RestartAnimation()
        {
            if (_animationSequence != null && _animationSequence.IsActive())
            {
                _animationSequence.Kill();
            }
            
            handContainer.position = new Vector3(handContainer.position.x, _initialYPosition, handContainer.position.z);
            _animationSequence = DOTween.Sequence()
                .SetLoops(-1)
                .Append(handContainer.DOMoveY(_initialYPosition - FixedMovementRange, pressDuration / 2)
                    .SetEase(Ease.InOutQuad))
                .Append(handContainer.DOMoveY(_initialYPosition, pressDuration / 2)
                    .SetEase(Ease.InOutQuad));
        }
        

        private Vector3 GetNextHandPosition()
        {
            float fixedZOffset = 0.2f;
            float direction = _addToRight ? 1f : -1f;
            _addToRight = !_addToRight;

            float newZ = mainHand.transform.position.z + direction * fixedZOffset;

            newZ = Mathf.Clamp(newZ, handData.minZ, handData.maxZ);

            return new Vector3(mainHand.transform.position.x, mainHand.transform.position.y, newZ);
        }
    }
}
