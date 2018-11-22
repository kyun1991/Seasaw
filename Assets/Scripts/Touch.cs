using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool dragging;
    private List<GameObject> spawned = new List<GameObject>();
    private int counter = 0;
    // private int tracker = 0;
    private float clickDelay = 0.25f;

    // initialise list.
    private void Start()
    {
        spawned = GameControl.instance.Spawned();
    }

    // hold down mouse to spawn objecitve.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameControl.instance.tracker == 0)
        {
            if (!GameControl.instance.noMoreObjective)
            {
                Time.timeScale = 0.7f;
                dragging = true;
                Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawned[counter].transform.position = new Vector2(tempPos.x, GameControl.instance.spawnHeight);
                spawned[counter].GetComponent<LineRenderer>().enabled = true;
                GameControl.instance.tracker++;
            }
        }
    }

    // release mouse to make objective fall and increment objective number.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameControl.instance.tracker == 1)
        {
            if (!GameControl.instance.noMoreObjective)
            {
                Time.timeScale = 1f;
                dragging = false;
                spawned[counter].GetComponent<Rigidbody2D>().isKinematic = false;
                spawned[counter].GetComponent<LineRenderer>().enabled = false;
                spawned[counter].GetComponent<PolygonCollider2D>().isTrigger = false;
                GameControl.instance.tracker++;
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
    IEnumerator ClickDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameControl.instance.tracker = 0;
        counter++;
        if (counter < spawned.Count)
        {
            spawned[counter].transform.position = new Vector2(0, GameControl.instance.spawnHeight);
        }
        PreviewInactive();
        if ((counter + 1) < spawned.Count)
        {
            PreviewActive(counter + 1);
        }
    }

    // activates preview for next object.
    public void PreviewActive(int nextObject)
    {
        for (int i = 0; i < GameControl.instance.preview.Length; i++)
        {
            if (spawned[nextObject].tag == GameControl.instance.preview[i].tag)
            {
                GameControl.instance.preview[i].SetActive(true);
            }
        }
    }

    // inactivates all previews.
    public void PreviewInactive()
    {
        for (int i = 0; i < GameControl.instance.preview.Length; i++)
        {
            GameControl.instance.preview[i].SetActive(false);
        }
    }
}
