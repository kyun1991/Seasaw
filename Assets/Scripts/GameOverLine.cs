using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checking to see if objects have fallen into water.
        if (collision.tag == "objects")
        {
            GameControl.instance.gameOverLine.SetActive(false);
            GameControl.instance.GameOver();            
        }
    }
}
