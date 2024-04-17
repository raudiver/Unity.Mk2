using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class raycastFromSquad : MonoBehaviour
{
    [SerializeField] Color colorToChange = Color.green;
    [SerializeField] Color defaultFieldColor = Color.green;

    GameObject objectThatHitting = null;
    GameObject objectThatWasHitting = null;
    string nameCurrentObject = "no object yet";

    listForRaycast parentScript;

    public void Start()
    {
        parentScript = transform.parent.gameObject.GetComponent<listForRaycast>();
        Debug.Log(parentScript);
    }



    void Update()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = new Vector3(0, 0, -1);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection);

        if (hit.collider != null && nameCurrentObject != hit.collider.name)
        {
            objectThatHitting = hit.collider.gameObject;
            parentScript.listWithHittedObjects.Add(objectThatHitting);

            if (objectThatWasHitting != null && objectThatWasHitting != objectThatHitting)//comment for highlight changes && !parentScript.listWithHittedObjects.Contains(objectThatWasHitting)
            {
                parentScript.listWithHittedObjects.Remove(objectThatWasHitting);
                
                if (!parentScript.listWithHittedObjects.Contains(objectThatWasHitting))
                {
                    objectThatWasHitting.GetComponent<SpriteRenderer>().color = defaultFieldColor;
                }
            }
            
            nameCurrentObject = hit.collider.name;
            hit.collider.GetComponent<SpriteRenderer>().color = colorToChange;
            objectThatWasHitting = hit.collider.gameObject;
        }
        else if (objectThatWasHitting != null && hit.collider == null)//comment for highlight changes &&!parentScript.listWithHittedObjects.Contains(objectThatWasHitting)
        {
            parentScript.listWithHittedObjects.Remove(objectThatWasHitting); //comment for highlight changes
            
            if (!parentScript.listWithHittedObjects.Contains(objectThatWasHitting))
            {
                objectThatWasHitting.GetComponent<SpriteRenderer>().color = defaultFieldColor;
            }

            nameCurrentObject = null;
            objectThatWasHitting = null;
        }

    }
}
