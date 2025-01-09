using System.Collections.Generic;
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
        private List<GameObject> _handLists = new List<GameObject>();
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
            _handLists.Add(mainHand); // Ana eli doğrudan listeye ekle
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
            int handsToRemove = Mathf.Min(count, _handLists.Count - 1);

            for (int i = 0; i < handsToRemove; i++)
            {
                var handToRemove = _handLists[_handLists.Count - 1];
                _handLists.RemoveAt(_handLists.Count - 1);
                Destroy(handToRemove);
            }
        }

        private Vector3 GetNextHandPosition()
        {
            float offset = 0.5f; // Eller arasındaki mesafe
            Vector3 basePosition = _handLists[0].transform.position; // Ana elin pozisyonu

            int handIndex = _handLists.Count; // Yeni elin indeksi
            float direction = handIndex % 2 == 0 ? 1f : -1f; // Çiftse sağa, tekse sola

            float xPosition = basePosition.x + (direction * offset * ((handIndex + 1) / 2));
            return new Vector3(xPosition, basePosition.y, basePosition.z);
        }
    }
}
