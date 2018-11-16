using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool dragging;
    private List<GameObject> spawned = new List<GameObject>();
    private int counter = 0;
    private float spawnHeight = 3f;

    // hold down mouse to spawn objecitve.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameControl.instance.noMoreObjective)
        {
            dragging = true;
            Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawned.Add(Instantiate(GameControl.instance.shell, new Vector2(tempPos.x, spawnHeight), Quaternion.identity));
            spawned[counter].GetComponent<Rigidbody2D>().isKinematic = true;
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
