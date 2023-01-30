using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLook : MonoBehaviour
{
    private float mouseSensitivity;

    [SerializeField] private Transform _playerBody;

    private float xRotation;

    private float mouseX;
    private float mouseY;

    private void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("sensitivity");
    }

    private void Look(Touch touch)
    {
        Vector2 touchPosition = touch.deltaPosition; 
        if (touch.phase == TouchPhase.Moved)
        {
            mouseX = touchPosition.x;
            mouseY = touchPosition.y;
                
            mouseX *= mouseSensitivity;
            mouseY *= mouseSensitivity;

            xRotation -= mouseY * Time.fixedDeltaTime;
            xRotation = Mathf.Clamp(xRotation, -80, 80);
        
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
            _playerBody.Rotate(Vector3.up * mouseX * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        Look(touch);
                    }
                }
            }
            else if (Input.touchCount == 1)
            {
                if (IsPointerOverUIObject())
                    return;
                
                Touch touch = Input.GetTouch(0);
                Look(touch);
            }
        }
    }
    
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
