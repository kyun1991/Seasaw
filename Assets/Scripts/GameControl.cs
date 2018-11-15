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
    public GameObject panelGameOver;
    public GameObject panelGameWin;
    public GameObject canvasInGame;
    public GameObject canvasMain;

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
            panelGameWin.SetActive(true);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        noMoreObjective = true;

        StartCoroutine(DeathTimer(1)); 
    }

    IEnumerator DeathTimer(float delay)
    {
        LevelControl.instance.StageReset();
        yield return new WaitForSeconds(delay);
        panelGameOver.SetActive(true);
    }

    public void ButtonPlayGameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonPlayGameWin()
    {        
        SceneManager.LoadScene(0);
        Scene scene = SceneManager.GetActiveScene();
        if (scene.isLoaded)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        canvasMain.SetActive(false);
        canvasInGame.SetActive(true);
    }
}