using UnityEngine;
using Data;

namespace Hand
{
    public class HandMovement : MonoBehaviour
    {
        [Header("~~~~~~~~~ MOVEMENT SETTINGS ~~~~~~~~~")]
        private Vector3 _dragStartPos;
        private Vector3 _currentDragPos;
        private bool _isMovingForward = true;

        [Header("~~~~~~~~~ MOVEMENT ELEMENTS ~~~~~~~~~")]
        [SerializeField] private HandData handData;
        private UnityEngine.Camera _mainCamera;

        private void Start()
        {
            _mainCamera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (!this.enabled) return;
            
            MoveForward();
            HandleDrag();
        }
        
        private void MoveForward()
        {
            if (_isMovingForward)
            {
                float moveX = handData.forwardSpeed * Time.deltaTime;
                transform.Translate(Vector3.right * moveX, Space.World);
            }
        }

        private void HandleDrag()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    StartDrag(touch.position);

                if (touch.phase == TouchPhase.Moved)
                    PerformDrag(touch.position);
            }
        }

        private void StartDrag(Vector2 touchPosition)
        {
            _dragStartPos = GetWorldPosition(touchPosition);
        }

        private void PerformDrag(Vector2 touchPosition)
        {
            _currentDragPos = GetWorldPosition(touchPosition);
            DragMove();
        }

        private void DragMove()
        {
            float moveAmountZ = (_currentDragPos.z - _dragStartPos.z) * handData.speed;
            
            float desiredZ = transform.position.z + moveAmountZ;
            
            float clampedZ = Mathf.Clamp(desiredZ, handData.minZ, handData.maxZ);
            
            if ((clampedZ == handData.minZ && moveAmountZ < 0) || (clampedZ == handData.maxZ && moveAmountZ > 0))
            {
                return;
            }
            float actualMoveZ = clampedZ - transform.position.z;
            HandManager.Instance.handContainer.Translate(new Vector3(0f, 0f, actualMoveZ), Space.World);
            
            _dragStartPos = _currentDragPos;
        }
        
        

        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            return transform.position;
        }
    }
}
