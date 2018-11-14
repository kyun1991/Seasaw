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

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                
        spawned.Add(Instantiate(GameControl.instance.shell, new Vector2(tempPos.x, spawnHeight), Quaternion.identity));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        spawned[counter].GetComponent<Rigidbody2D>().isKinematic = false;
        spawned[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
    }

    private void Update()
    {
        if (dragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawned[counter].transform.position = new Vector2(mousePosition.x, spawnHeight);                
        }
    }
}
