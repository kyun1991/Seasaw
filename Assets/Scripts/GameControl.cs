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
    public GameObject PanelGameOver;   

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
        LevelControl.instance.StageUpText();
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
            LevelControl.instance.StageUp();
            Debug.Log("WINNER WINNER CHICKEN DINNER");

            SceneManager.LoadScene(0);
            // ACTIVATE STAGE CLEAR PANEL @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // PRESS BUTTON TO GO ONTO NEXT STAGE ETC
        }
    }

    public void GameOver()
    {
        gameOver = true;
        noMoreObjective = true;

        StartCoroutine(DeathTimer(2)); 
    }

    IEnumerator DeathTimer(float delay)
    {
        LevelControl.instance.StageReset();
        yield return new WaitForSeconds(delay);
        PanelGameOver.SetActive(true);
    }

    public void ButtonPlay()
    {
        SceneManager.LoadScene(0);
    }
}