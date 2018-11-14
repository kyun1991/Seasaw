using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public GameObject shell;
    public GameObject gameOverLine;

    public Text TextObjectiveNumber;
    public Text TextStage;

    public bool noMoreObjective;

    private bool gameOver;
    private int objectiveNumber = 5;
    
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
    void Start()
    {
        TextStage.text = "Stage " + PlayerPrefs.GetInt("stage", 1);
        TextObjectiveNumber.text = objectiveNumber.ToString();
    }

    public void IncrementObjective()
    {
        objectiveNumber--;
        TextObjectiveNumber.text = objectiveNumber.ToString();

        if (objectiveNumber == 0)
        {
            noMoreObjective = true;

            StartCoroutine(StageClear(3f));
        }
    }

    IEnumerator StageClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverLine.SetActive(false);

        if (gameOver == false)
        {
            PlayerPrefs.SetInt("stage", PlayerPrefs.GetInt("stage", 1) + 1);
            Debug.Log("WINNER WINNER CHICKEN DINNER");

            // ACTIVATE STAGE CLEAR PANEL @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // PRESS BUTTON TO GO ONTO NEXT STAGE ETC
        }
    }

    public void GameOver()
    {
        gameOver = true;
        PlayerPrefs.SetInt("stage", 1);
        Debug.Log("GAMEOVER");

        StartCoroutine(DeathTimer(2));    
    }

    IEnumerator DeathTimer(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ACTIVATE DIE PANEL. HOME & RESTART BUTTON ETC. @@@@@@@@@@@@@@@@@@@
    }
}