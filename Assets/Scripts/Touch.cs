using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool dragging;
    private List<GameObject> spawned = new List<GameObject>();
    private int counter = 0;
    private int tracker = 0;
    private int activePreview;
    private float clickDelay=0.25f;

    private List<string> previewString = new List<string>();

    // initialise list.
    private void Start()
    {
        spawned = GameControl.instance.Spawned();
        
        // initialise list of strings to store tags in preview array.
        for (int i = 0; i < GameControl.instance.preview.Length; i++)
        {
            previewString.Add(GameControl.instance.preview[i].tag);
        }

        // check if next object has same tag as any object from preview array. If it matches, assign that index to activePreview.
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[1].tag == previewString[i])
            {
                activePreview = i;
            }
        }

        for (int i = 0; i < GameControl.instance.preview.Length; i++)
        {
            if (i != activePreview)
            {

            }
        }
       
    }

    // hold down mouse to spawn objecitve.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tracker == 0)
        {
            if (!GameControl.instance.noMoreObjective)
            {
                Time.timeScale = 0.6f;
                dragging = true;
                Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawned[counter].transform.position = new Vector2(tempPos.x, GameControl.instance.spawnHeight);
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
            spawned[counter].transform.position = new Vector2(mousePosition.x, GameControl.instance.spawnHeight);
        }
    }

    // used to delay time between clicks and position next object in middle of screen.
    IEnumerator ClickDelay(float delay){
        yield return new WaitForSeconds(delay);
        tracker = 0;
        counter++;
        if (counter < spawned.Count)
        {
            spawned[counter].transform.position = new Vector2(0, GameControl.instance.spawnHeight);
        }

    }
}
