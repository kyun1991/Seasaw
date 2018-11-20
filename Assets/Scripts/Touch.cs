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

    private int tracker = 0;
    private float clickDelay=0.3f;

    // initialise list.
    private void Start()
    {
        spawned = GameControl.instance.Spawned();
    }

    // hold down mouse to spawn objecitve.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tracker == 0)
        {
            if (!GameControl.instance.noMoreObjective)
            {
                Time.timeScale = 0.1f;
                dragging = true;
                Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawned[counter].transform.position = new Vector2(tempPos.x, spawnHeight);
                spawned[counter].GetComponent<LineRenderer>().enabled = true;
                tracker++;
            }
        }
    }

    // release mouse to make objective fall and increment objective number.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (tracker == 1)
        {
            if (!GameControl.instance.noMoreObjective)
            {
                Time.timeScale = 1f;
                dragging = false;
                spawned[counter].GetComponent<Rigidbody2D>().isKinematic = false;
                spawned[counter].GetComponent<LineRenderer>().enabled = false;
                spawned[counter].GetComponent<PolygonCollider2D>().isTrigger = false;
                counter++;
                tracker++;
                GameControl.instance.IncrementObjective();
                StartCoroutine(ClickDelay(clickDelay));
            }
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

    // used to delay time between clicks.
    IEnumerator ClickDelay(float delay){
        yield return new WaitForSeconds(delay);
        tracker = 0;
    }
}
