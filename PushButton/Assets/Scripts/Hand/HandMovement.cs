using System;
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
         transform.Translate(Vector3.forward * (handData.forwardSpeed * Time.deltaTime),Space.World);
   }

   private void HandleDrag()
   {
      if (Input.GetMouseButtonDown(0))
         StartDrag();

      if (Input.GetMouseButton(0))
         PerformDrag();
   }

   private void StartDrag()
   {
      _dragStartPos = GetMouseWorldPosition();
   }

   private void PerformDrag()
   {
      _currentDragPos = GetMouseWorldPosition();
      float moveAmount = _currentDragPos.x - _dragStartPos.x;
      
      Vector3 newPosition = transform.position + new Vector3(moveAmount * handData.speed * Time.deltaTime, 0f, 0f);
      newPosition.x = Mathf.Clamp(newPosition.x, handData.minX, handData.maxX);
      newPosition.y = transform.position.y;
      newPosition.z = transform.position.z;
      
      transform.position = newPosition;
      _dragStartPos = _currentDragPos;
   }

   private Vector3 GetMouseWorldPosition()
   {
      Vector3 mousePosition = Input.mousePosition;
      mousePosition.z = _mainCamera.WorldToScreenPoint(transform.position).z;
      return _mainCamera.ScreenToWorldPoint(mousePosition);
   }
}
