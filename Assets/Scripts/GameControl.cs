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
    public GameObject sliderTimer;

    public Slider timer;

    public Text TextObjectiveNumber;
    public Text TextCurrentStage;
    public Text TextCurrentScore;
    public Text TextHighStageAndScore;
    
    public bool noMoreObjective;

    private bool gameOver;
    private bool startTimer;
    private int objectiveNumber = 5;
    private float stageClearDelay = 3f;
    private float tempTime = 0;

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
        LevelControl.instance.IncrementStageText();
        LevelControl.instance.IncrementScoreText();
        TextObjectiveNumber.text = objectiveNumber.ToString();
        TextHighStageAndScore.text = "Stage " + PlayerPrefs.GetInt("highstage", 1) + " , " + PlayerPrefs.GetInt("highscore", 0);
        Time.timeScale = 1;

        // if level is continued, start game without showing main menu.
        if (LevelControl.instance.stageContinued == true)
        {
            StartGame();
        }
    }

    private void Update()
    {
        // starts timer once all objectives spawned.
        if (startTimer == true)
        {
            tempTime += Time.deltaTime;
            timer.value = (stageClearDelay - tempTime) / stageClearDelay;

            // if timer reaches zero, activate gamewin panel to progress onto next stage.
            if (timer.value <= 0)
            {
                gameOverLine.SetActive(false);
                Time.timeScale = 0.1f;
                sliderTimer.SetActive(false);
                panelGameWin.SetActive(true);
            }
        }
    }

    // called from Touch script on mouse up to decrement objective number and update objective number text.
    public void IncrementObjective()
    {
        objectiveNumber--;
        TextObjectiveNumber.text = objectiveNumber.ToString();
        LevelControl.instance.IncrementScore();

        // if objective = 0 , stop spawning objective on click, and start countdown for stageclear.
        if (objectiveNumber == 0)
        {
            noMoreObjective = true;
            startTimer = true;
            sliderTimer.SetActive(true);
            LevelControl.instance.IncrementStage();
        }
    }

    // called from GameOverLine script when objective touches water.
    public void GameOver()
    {
        startTimer = false;
        gameOver = true;
        noMoreObjective = true;
        StartCoroutine(DeathTimer(0.5f));
    }

    // resets stage number to 1 and activates gameover panel after x seconds.
    IEnumerator DeathTimer(float delay)
    {
        LevelControl.instance.StageReset();
        yield return new WaitForSeconds(delay);
        panelGameOver.SetActive(true);
    }

    // load scene when ButtonPlay is clicked.
    public void ButtonPlay()
    {
        SceneManager.LoadScene(0);
    }

    // activiate/deactivate canvas when game starts.
    public void StartGame()
    {
        canvasMain.SetActive(false);
        canvasInGame.SetActive(true);
    }
}