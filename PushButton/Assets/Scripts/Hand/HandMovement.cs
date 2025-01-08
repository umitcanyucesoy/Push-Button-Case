using System.Collections;
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
            transform.Translate(Vector3.forward * (handData.forwardSpeed * Time.deltaTime), Space.World);
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

        // X ekseni sınırlarını uygula
        newPosition.x = Mathf.Clamp(newPosition.x, handData.minX, handData.maxX);

        // Y ve Z eksenlerini sabit tut
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;

        // Yeni pozisyona taşı
        transform.position = newPosition;

        // Sadece drag sırasında pozisyonu güncelle
        _dragStartPos = _currentDragPos;
    }

    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        // Kamera hesaplamalarını optimize etmek için z eksenini bir kere hesaplayın
        float zPosition = _mainCamera.WorldToScreenPoint(transform.position).z;
        screenPosition.z = zPosition;

        return _mainCamera.ScreenToWorldPoint(screenPosition);
    }

}
