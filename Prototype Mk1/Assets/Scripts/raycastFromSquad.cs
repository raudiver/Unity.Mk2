using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class raycastFromSquad : MonoBehaviour
{
    [SerializeField] Color colorToChange = Color.green;
    [SerializeField] Color defaultFieldColor = Color.green;
    bool wasHit = false;
    GameObject objectThatHitting = null;
    GameObject objectThatWasHitting = null;
    string nameCurrentObject = "no object yet";



    void Update()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = new Vector3(0, 0, -1);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection);

        if (hit.collider != null && nameCurrentObject != hit.collider.name)
        {
            objectThatHitting = hit.collider.gameObject;

            if (objectThatWasHitting != null && objectThatWasHitting != objectThatHitting)
            {
                objectThatWasHitting.GetComponent<SpriteRenderer>().color = defaultFieldColor;
            }
            
            nameCurrentObject = hit.collider.name;
            hit.collider.GetComponent<SpriteRenderer>().color = colorToChange;
            objectThatWasHitting = hit.collider.gameObject;
        }
        else if (objectThatWasHitting != null && hit.collider == null)
        {
            objectThatWasHitting.GetComponent<SpriteRenderer>().color = defaultFieldColor;
            nameCurrentObject = null;
        }

    }
}
