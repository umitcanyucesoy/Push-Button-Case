using System.Collections.Generic;
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
        private readonly List<GameObject> _handLists = new List<GameObject>();
        private const int MaxHands = 5;

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
        }

        public void AddHands(int count)
        {
            int handsToAdd = Mathf.Min(count, MaxHands - _handLists.Count);

            for (int i = 0; i < handsToAdd; i++)
            {
                var newHand = Instantiate(handPrefab, GetNextHandPosition(), handContainer.rotation, handContainer);
                _handLists.Add(newHand);
            }

            UpdateHandPositions();
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

            UpdateHandPositions();
        }


        public void UpdateHandPositions()
        {
            float offset = 0.21f;
            Vector3 basePosition = _handLists[0].transform.position;

            for (int i = 1; i < _handLists.Count; i++)
            {
                float direction = i % 2 == 0 ? 1f : -1f;
                float xPosition = basePosition.x + (direction * offset * ((i + 1) / 2));

                Vector3 newPosition = new Vector3(xPosition, basePosition.y, basePosition.z);
                _handLists[i].transform.position = newPosition;
            }
        }

        private Vector3 GetNextHandPosition()
        {
            float offset = 0.21f;
            Vector3 basePosition = _handLists[0].transform.position;
            int handIndex = _handLists.Count;
            float direction = handIndex % 2 == 0 ? 1f : -1f;
            float xPosition = basePosition.x + (direction * offset * ((handIndex + 1) / 2));

            return new Vector3(xPosition, basePosition.y, basePosition.z);
        }
    }
}
