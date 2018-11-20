using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool dragging;
    private List<GameObject> spawned = new List<GameObject>();
    private int counter = 0;
    private int objectiveNumber;
    private float spawnHeight = 3f;

    // initialise list and objective number.
    private void Start()
    {
        spawned = GameControl.instance.Spawned();
        objectiveNumber = GameControl.instance.ObjectiveNumber();
    }

    // hold down mouse to spawn objecitve.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameControl.instance.noMoreObjective)
        {
            dragging = true;
            Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);          
            spawned[counter].transform.position = new Vector2(tempPos.x, spawnHeight);
            spawned[counter].GetComponent<LineRenderer>().enabled = true;

            // positions the next objective to be spawned on top of PreviewPos gameobject.
            if (counter < (objectiveNumber-1))
            {
                spawned[counter + 1].transform.position = GameControl.instance.previewPos.transform.position;
            }
        }
    }

    // release mouse to make objective fall and increment objective number.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameControl.instance.noMoreObjective)
        {
            dragging = false;
            spawned[counter].GetComponent<Rigidbody2D>().isKinematic = false;
            spawned[counter].GetComponent<LineRenderer>().enabled = false;
            spawned[counter].GetComponent<PolygonCollider2D>().isTrigger = false;
            counter++;
            GameControl.instance.IncrementObjective();
        }
    }

    // if mouse is down and moving, adjust object position to mouse position
    private void Update()
    {
        if (dragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawned[counter].transform.position = new Vector2(mousePosition.x, spawnHeight);
        }
    }
}
