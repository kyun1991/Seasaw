using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(GameControl.instance.shell, mousePosition, Quaternion.identity);
        GameControl.instance.shell.GetComponent<Rigidbody2D>().isKinematic = true;
        
     
        GameControl.instance.lr.enabled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameControl.instance.lr.enabled = false;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(GameControl.instance.shell, mousePosition, Quaternion.identity);
        GameControl.instance.shell.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
