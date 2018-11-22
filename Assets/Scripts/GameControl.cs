﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public GameObject[] fish;
    public GameObject[] preview;
    public SpriteRenderer[] stageIndicator;

    public GameObject stageIndicatorGO;
    public GameObject gameOverLine;
    public GameObject panelGameOver;
    public GameObject panelGameWin;
    public GameObject canvasInGame;
    public GameObject canvasMain;
    public GameObject sliderTimer;
    public GameObject platform;
    public GameObject bossStageAnimation;

    // game variety variables
    public Vector2 platformPos;
    public float platformMass;
    public float platformAngularDrag;
    private int objectiveNumber;

    public Slider timer;

    public Text TextObjectiveNumber;
    public Text TextCurrentStage;
    public Text TextCurrentScore;
    public Text TextHighStageAndScore;

    public float spawnHeight;

    public int tracker = 10;

    public bool noMoreObjective;
    public bool startSpawning;

    private bool startTimer;
    private int bossCounter;
    private int currentStage;
    private float stageClearDelay = 2.5f;
    private float tempTime = 0;

    private List<GameObject> spawned = new List<GameObject>();

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
        platform.GetComponent<Rigidbody2D>().mass = platformMass;
        platform.GetComponent<Rigidbody2D>().angularDrag = platformAngularDrag;

        currentStage = LevelControl.instance.StageReturn();
        StageControl(currentStage);
        int whatStage = LevelControl.instance.StageReturn() % LevelControl.instance.BossFreqReturn();

        if (whatStage != 0)
        {
            startSpawning = true;
        }

        for (int i = 0; i < whatStage; i++)
        {
            stageIndicator[i].color = new Color32(226, 87, 76, 255);
        }

        LevelControl.instance.IncrementStageText();
        LevelControl.instance.IncrementScoreText();

        TextHighStageAndScore.text = "Stage " + PlayerPrefs.GetInt("highstage", 1) + " , " + PlayerPrefs.GetInt("highscore", 0);

        // if boss stage, then initialise boss attack depending on what boss it is.
        if (LevelControl.instance.boss == true)
        {
            GameObject temp = Instantiate(bossStageAnimation, new Vector2(0, 0.5f), Quaternion.identity);
            Destroy(temp, 2f);

            for (int i = 0; i < stageIndicator.Length; i++)
            {
                stageIndicator[i].color = new Color32(226, 87, 76, 255);
            }

            // depending on which boss it is, objective numbers differ.
            bossCounter = LevelControl.instance.StageReturn() / LevelControl.instance.BossFreqReturn();
            if (bossCounter == 1)
            {
                objectiveNumber = 5;
            }
            else if (bossCounter == 2)
            {
                objectiveNumber = 5;
            }
            else if (bossCounter == 3)
            {
                objectiveNumber = 5;
            }
            else if (bossCounter == 4)
            {
                objectiveNumber = 5;
            }
            else if (bossCounter == 5)
            {
                objectiveNumber = 5;
            }
            else { objectiveNumber = 5; }
        

            StartCoroutine(StartBossAttack(1.5f,bossCounter));
        }
        // creates a list of objectives that will be used in current stage.
        for (int i = 0; i < objectiveNumber; i++)
        {
            spawned.Add(Instantiate(fish[Random.Range(0, fish.Length)], new Vector2(0, 10), Quaternion.identity));
            spawned[i].GetComponent<Rigidbody2D>().isKinematic = true;
        }
        // if level is continued, start game without showing main menu.
        if (LevelControl.instance.stageContinued == true)
        {
            StartGame();
        }
        // adjust platform anchor and objective text to new position.
        platform.GetComponent<HingeJoint2D>().anchor = platformPos;
        platform.GetComponentInChildren<RectTransform>().localPosition = platformPos;
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
        TextObjectiveNumber.text = objectiveNumber.ToString();
        stageIndicatorGO.SetActive(true);
        CheckStartSpawning();
    }

    // returns list of spawned objectives. Used in Touch script.
    public List<GameObject> Spawned()
    {
        return spawned;
    }

    // changing difficulty of stages.
    public void StageControl(int stage)
    {
        if (stage < 4)
        {
            objectiveNumber = 4;
        }
        if (4 < stage && stage < 8)
        {
            objectiveNumber = 6;
        }
        if (8 < stage && stage < 12)
        {
            objectiveNumber = 8;
        }
        if (12 < stage && stage % LevelControl.instance.BossFreqReturn() != 0)
        {
            objectiveNumber = 10;
        }
    }

    // brief delay to show boss stage animation before starting boss attack.
    IEnumerator StartBossAttack(float delay, int bossCounter)
    {


        yield return new WaitForSeconds(delay);
        startSpawning = true;
        CheckStartSpawning();

        if (bossCounter == 1)
        {
            yield return StartCoroutine(GetComponent<BossControl>().BossOne());
        }
        else if (bossCounter == 2)
        {
            yield return StartCoroutine(GetComponent<BossControl>().BossTwo());
        }
        else if (bossCounter == 3)
        {
            yield return StartCoroutine(GetComponent<BossControl>().BossThree());
        }
        else if (bossCounter == 4)
        {
            yield return StartCoroutine(GetComponent<BossControl>().BossFour());
        }
        else if (bossCounter == 5)
        {
            yield return StartCoroutine(GetComponent<BossControl>().BossFive());
        }

    }

    // if startSpawning is true, sets first object and first preview object into position.
    public void CheckStartSpawning()
    {
        if (startSpawning == true)
        {
            tracker = 0;

            spawned[0].transform.position = new Vector2(0, spawnHeight);
            for (int i = 0; i < preview.Length; i++)
            {
                if (spawned[1].tag == preview[i].tag)
                {
                    preview[i].SetActive(true);
                }
            }
        }
    }
}