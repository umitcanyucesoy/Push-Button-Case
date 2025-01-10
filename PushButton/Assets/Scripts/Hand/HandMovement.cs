using System.Collections.Generic;
using Data;
using Hand;
using UnityEngine;

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
        MoveForward();
        HandleDrag();
    }
    
    private void MoveForward()
    {
        if (_isMovingForward)
        {
            transform.Translate(Vector3.right * (handData.forwardSpeed * Time.deltaTime), Space.World); // Z ekseni yerine X ekseni
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
        _dragStartPos = GetMouseWorldPosition(touchPosition);
    }

    private void PerformDrag(Vector2 touchPosition)
    {
        _currentDragPos = GetMouseWorldPosition(touchPosition);
        DragMove();
    }

    private void DragMove()
    {
        float moveAmount = (_currentDragPos.z - _dragStartPos.z) * handData.speed;
        
        if (HandManager.Instance.IsAtBoundary(moveAmount))
            return;

        Vector3 newPosition = transform.position + new Vector3(0f, 0f, moveAmount);
        newPosition.z = Mathf.Clamp(newPosition.z, handData.minZ, handData.maxZ);
        newPosition.y = transform.position.y;
        newPosition.x = transform.position.x;

        transform.position = newPosition;
        _dragStartPos = _currentDragPos;
    }

    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        screenPosition.z = _mainCamera.WorldToScreenPoint(transform.position).z;
        return _mainCamera.ScreenToWorldPoint(screenPosition);
    }
}