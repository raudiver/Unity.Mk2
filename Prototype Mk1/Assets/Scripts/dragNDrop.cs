using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class dragNDrop : MonoBehaviour
{
    /*This class will contain all functionality for objects that can be dragged and dropped by mouse.
     *We will transfrom object pos after mouseDown and stop transfroming for mouse up
     Also we will use pos delta to avoid object teleportation after first click*/

    Vector2 mouseWorldPosition; 
    Vector2 startMousePos;
    Vector2 objectStartPosition;
    bool dndProcessInProgress = false;
    Mouse mouse;
    raycastFromSquad childRaycastFromSquad;
    PolygonCollider2D colliderParent;

    private void Start()
    {
        mouse = Mouse.current;
        childRaycastFromSquad = GetComponentInChildren<raycastFromSquad>();
        Debug.Log(childRaycastFromSquad);
        colliderParent = GetComponent<PolygonCollider2D>();


    }
    void Update()
    {
        //we need to store pointer pos and tranfrom it from screen coordinates to world coordinates
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue()); //we set every frame mouse pos in world coordinates

        if (mouse.leftButton.wasPressedThisFrame && IsMouseOverObject())
        {
            objectStartPosition = transform.position;
            startMousePos = mouseWorldPosition;
            dndProcessInProgress = true;
        }

        if (mouse.leftButton.IsPressed() && dndProcessInProgress)
        {  
            Vector2 deltaPosition = startMousePos - mouseWorldPosition;
            transform.position = objectStartPosition - deltaPosition;
            //transform.localScale = new Vector3(0.8f, 0.8f, 0);
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            dndProcessInProgress = false;
            //transform.localScale = new Vector3(1f, 1f, 0);
            if (IsMouseOverObject())
            {
                transform.position = transform.position - childRaycastFromSquad.GetClosesDistanceToCell();
            }
        }
    }

    bool IsMouseOverObject()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
        return colliderParent.OverlapPoint(mousePosition);
    }

    void OnRotate(InputValue value)
    {
        if (IsMouseOverObject() && mouse.leftButton.IsPressed())
        {
            transform.Rotate(Vector3.forward * 90f);
        }
    }
}

//mouse.leftButton.IsPressed - TRUE when button pressed and not released