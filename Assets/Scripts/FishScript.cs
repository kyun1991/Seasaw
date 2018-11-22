using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    private bool Dropped = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Dropped)
        {
            GameControl.instance.Drop.Play();
            Dropped = true;
        }
    }
}
