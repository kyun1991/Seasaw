using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour {

    public static LevelControl instance;

    private int stage=1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}

    public void StageUp()
    {
        stage++;     
    }

    public void StageUpText()
    {
        GameControl.instance.TextStage.text = "Stage " + stage;
    }

    public void StageReset()
    {
        stage = 1;
    }
}
