using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
        parentScript = transform.parent.gameObject.GetComponent<listForRaycast>(); //parent screept for saving list with game.objects
        Mouse mouse = Mouse.current;
    }

    void Update()
    {
        if (Mouse.current.leftButton.IsPressed())
        {
            ColoringCelssGreen();
        }
    }

    private void ColoringCelssGreen()
    {
        /* We will uuse Rasyacst functionality for finding part of the field under the senter of squad.
        * For highlightning we will use that list that contains all game.objects under the all units in squad
        * We will use these noject from the list for changing color to green */
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = new Vector3(0, 0, -1);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection);

        if (hit.collider != null && nameCurrentObject != hit.collider.name)
        {
            objectThatHitting = hit.collider.gameObject;
            parentScript.listWithHittedObjects.Add(objectThatHitting);

            if (objectThatWasHitting != null && objectThatWasHitting != objectThatHitting)
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
        else if (objectThatWasHitting != null && hit.collider == null)
        {
            parentScript.listWithHittedObjects.Remove(objectThatWasHitting);

            if (!parentScript.listWithHittedObjects.Contains(objectThatWasHitting))
            {
                objectThatWasHitting.GetComponent<SpriteRenderer>().color = defaultFieldColor;
            }

            nameCurrentObject = null;
            objectThatWasHitting = null;
        }
    }
}
