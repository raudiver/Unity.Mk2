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
    Vector2 lastKnownPosition;
    Quaternion starRotationPosition;
    bool dndProcessInProgress = false;
    Mouse mouse;
    raycastFromSquad childRaycastFromSquad;
    PolygonCollider2D colliderParent;
    int amountOfChild;
    listForRaycast parentScript;
    public string layerName = "Squad";
    LayerMask squadLayerMask;

    private void Start()
    {
        mouse = Mouse.current;
        childRaycastFromSquad = GetComponentInChildren<raycastFromSquad>();
        Debug.Log(childRaycastFromSquad);
        colliderParent = GetComponent<PolygonCollider2D>();
        amountOfChild = transform.childCount;
        parentScript = transform.gameObject.GetComponent<listForRaycast>();
        squadLayerMask = LayerMask.GetMask(layerName);


    }
    void Update()
    {
        //we need to store pointer pos and tranfrom it from screen coordinates to world coordinates
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue()); //we set every frame mouse pos in world coordinates

        if (mouse.leftButton.wasPressedThisFrame && IsMouseOverObject())
        {
            objectStartPosition = transform.position;
            starRotationPosition = transform.rotation;
            startMousePos = mouseWorldPosition;
            dndProcessInProgress = true;
        }

        if (mouse.leftButton.IsPressed() && dndProcessInProgress)
        {  
            Vector2 deltaPosition = startMousePos - mouseWorldPosition;
            transform.position = objectStartPosition - deltaPosition;
            //transform.localScale = new Vector3(0.8f, 0.8f, 0);
        }

        if (mouse.leftButton.wasReleasedThisFrame && dndProcessInProgress)
        {
            dndProcessInProgress = false;
                if (amountOfChild == parentScript.listWithHittedObjects.Count)
                {
                    transform.position = transform.position - childRaycastFromSquad.GetClosesDistanceToCell();
                }
                else
                {
                    transform.position = objectStartPosition;
                    transform.rotation = starRotationPosition;
                    foreach (Transform child in transform)
                    {
                        raycastFromSquad raycastFromSquad = child.GetComponent<raycastFromSquad>();
                        raycastFromSquad.ColoringCelssGreen();
                    }
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
        if (dndProcessInProgress && mouse.leftButton.IsPressed())//DEBUG if (IsMouseOverObject() && mouse.leftButton.IsPressed())
        {
            transform.Rotate(Vector3.forward * 90f);
        }
    }

    public GameObject MouseWasClickedOnObject()
    {
        Vector2 rayOrigin = startMousePos;
        Vector2 rayDirection = new Vector3(0, 0, 1);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, Mathf.Infinity);
        if (hit.collider != null)
        {
            GameObject objectHittedByMouse = hit.collider.gameObject;
            return objectHittedByMouse;
        }
        else
        {
            return null;
        }
    }
}

//mouse.leftButton.IsPressed - TRUE when button pressed and not released
// fix bug, during rotation if mouse not under the object - didn't place - FIXED
// fix bug, during rotation if mouse not on object the object stops rotating - FIXED
// fix bug, if mouse under the two object - rotates 
// tranform.GameObject == hit.collider GameObject