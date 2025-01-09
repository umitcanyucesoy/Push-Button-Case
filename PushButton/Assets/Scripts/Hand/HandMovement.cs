using System.Collections.Generic;
using Data;
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
            transform.Translate(Vector3.forward * (handData.forwardSpeed * Time.deltaTime), Space.World);
            Hand.HandManager.Instance.UpdateHandPositions(); 
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
        float moveAmount = (_currentDragPos.x - _dragStartPos.x) * handData.speed;

        Vector3 newPosition = transform.position + new Vector3(moveAmount, 0f, 0f);
        newPosition.x = Mathf.Clamp(newPosition.x, handData.minX, handData.maxX);
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;

        transform.position = newPosition;
        _dragStartPos = _currentDragPos;

        Hand.HandManager.Instance.UpdateHandPositions();
    }

    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        screenPosition.z = _mainCamera.WorldToScreenPoint(transform.position).z;
        return _mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
