using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public GameObject[] fish;
    public GameObject[] preview;
    public SpriteRenderer[] stageIndicator;

    public GameObject stageIndicatorGO;
    public GameObject stackCount;
    public GameObject guide;
    public GameObject preventTouchButton;
    public GameObject platform;
    public GameObject gameOverLine;
    public GameObject panelGameOver;
    public GameObject panelGameWin;
    public GameObject canvasInGame;
    public GameObject canvasMain;
    public GameObject title;
    public GameObject sliderTimer;
    public GameObject bossStageAnimation;
    public GameObject imageStageClear;
    public GameObject imageStageGreat;
    public GameObject buttonUnmute;
    public GameObject buttonMute;
    public GameObject GA;

    //Audio objects
    public AudioSource Splash;
    public AudioSource Drop;
    public AudioSource Clear;

    // game variety variables
    public float platformMass;
    public float platformAngularDrag;
    private int objectiveNumber;

    public Slider timer;

    public Text TextObjectiveNumber;
    public Text TextCurrentStage;
    public Text TextCurrentScore;
    public Text TextHighStage;
    public Text TextHighScore;
    public Text deathPanelStage;
    public Text deathPanelScore;

    public float spawnHeight;

    public int tracker = 10;

    public bool noMoreObjective;
    public bool startSpawning;

    private bool startTimer;
    private bool canIncrementStage;
    private int bossCounter;
    private int currentStage;
    private int whaleIndex;
    private int whaleCount = 0;

    private float stageClearDelay;
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
        Application.targetFrameRate = 300;
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            UnMute();
        }
        else
        {
            Mute();
        }

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

        // setting platform angular velocity according to stage.
        if (currentStage > 0 && currentStage < 5)
        {
            platformAngularDrag = 4f;
        }

        else if (currentStage > 4 && currentStage < 9)
        {
            platformAngularDrag = 6f;
        }

        else if (currentStage > 8 && currentStage < 13)
        {
            platformAngularDrag = 8f;
        }

        else
        {
            platformAngularDrag = 10f;
        }

        platform.GetComponent<Rigidbody2D>().mass = platformMass;
        platform.GetComponent<Rigidbody2D>().angularDrag = platformAngularDrag;

        LevelControl.instance.IncrementStageText();
        LevelControl.instance.IncrementScoreText();

        TextHighStage.text = "Stage " + PlayerPrefs.GetInt("highstage", 1);
        TextHighScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();

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
                objectiveNumber = 4;
                stageClearDelay = 1.6f;
            }
            else if (bossCounter == 2)
            {
                objectiveNumber = 5;
                stageClearDelay = 1.9f;
            }
            else if (bossCounter == 3)
            {
                objectiveNumber = 5;
                stageClearDelay = 2.2f;
            }
            else if (bossCounter == 4)
            {
                objectiveNumber = 6;
                stageClearDelay = 2.5f;
            }
            else
            {
                objectiveNumber = 6;
                stageClearDelay = 2.5f;
            }
            StartCoroutine(StartBossAttack(1.5f, bossCounter));
        }

        FishIndex();

        // creates a list of objectives that will be used in current stage.
        for (int i = 0; i < objectiveNumber; i++)
        {
            int fishSpawnIndex = Random.Range(0, fish.Length);

            // prevents more than 2 whales spawning
            if (fishSpawnIndex == whaleIndex)
            {
                whaleCount++;
                if (whaleCount > 2)
                {
                    while (fishSpawnIndex == whaleIndex)
                    {
                        fishSpawnIndex = Random.Range(0, fish.Length);
                    }
                }
            }

            spawned.Add(Instantiate(fish[fishSpawnIndex], new Vector2(0, 10), Quaternion.identity));
            spawned[i].GetComponent<Rigidbody2D>().isKinematic = true;
        }

        // if level is continued, start game without showing main menu.
        if (LevelControl.instance.stageContinued == true)
        {
            StartGame();
        }

        GameObject analytics = GameObject.Find("/GameAnalytics");
        if (!analytics)
        {
            analytics = Instantiate(GA);
            analytics.name = "GameAnalytics";
            GameAnalytics.Initialize();
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
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
                sliderTimer.SetActive(false);
                panelGameWin.SetActive(true);


                if (!canIncrementStage)
                {

                    // checking whether to show CLEAR for normal stage and GREAT for boss stage.
                    if (LevelControl.instance.StageReturn() % LevelControl.instance.BossFreqReturn() == 0)
                    {
                        imageStageGreat.SetActive(true);
                        StartCoroutine(NextStage(1.1f));
                        Clear.Play();
                    }
                    else
                    {
                        imageStageClear.SetActive(true);
                        StartCoroutine(NextStage(0.8f));
                        Clear.Play();
                    }

                    LevelControl.instance.IncrementStage();
                    canIncrementStage = true;
                }
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
        }
    }

    // called from GameOverLine script when objective touches water.
    public void GameOver()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", LevelControl.instance.GetScore());
        startTimer = false;
        noMoreObjective = true;
        deathPanelStage.text = "Stage "+LevelControl.instance.StageReturn();
        StartCoroutine(DeathTimer(0.5f));
    }

    // resets stage number to 1 and activates gameover panel after x seconds.
    IEnumerator DeathTimer(float delay)
    {
        LevelControl.instance.StageReset();
        yield return new WaitForSeconds(delay);
        stageIndicatorGO.SetActive(false);
        TextCurrentScore.text = "";
        TextCurrentStage.text = "";
        panelGameOver.SetActive(true);
    }

    // load scene when ButtonPlay is clicked.
    public void ButtonPlay()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
        SceneManager.LoadScene(0);
    }

    // activiate/deactivate canvas when game starts.
    public void StartGame()
    {
        
        canvasMain.SetActive(false);
        title.SetActive(false);
        canvasInGame.SetActive(true);
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
            stageClearDelay = 1.6f;
        }
        if (4 < stage && stage < 8)
        {
            objectiveNumber = 6;
            stageClearDelay = 1.9f;
        }
        if (8 < stage && stage < 12)
        {
            objectiveNumber = 8;
            stageClearDelay = 2.2f;
        }
        if (12 < stage && stage % LevelControl.instance.BossFreqReturn() != 0)
        {
            objectiveNumber = 10;
            stageClearDelay = 2.5f;
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
        else
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

            TextObjectiveNumber.text = objectiveNumber.ToString();
            stackCount.SetActive(true);

            spawned[0].transform.position = new Vector2(0, spawnHeight);
            for (int i = 0; i < preview.Length; i++)
            {
                if (spawned[1].tag == preview[i].tag)
                {
                    preview[i].SetActive(true);
                }
            }

            // if this is first time for player, show guide with hand movement.
            if (PlayerPrefs.GetInt("firsttime", 0) == 0)
            {
                preventTouchButton.SetActive(true);
                Instantiate(guide, new Vector2(-0.58f, 1f), Quaternion.identity);
                StartCoroutine(DelayTouch(2f));
            }
        }
    }

    IEnumerator DelayTouch(float delay)
    {
        yield return new WaitForSeconds(delay);
        preventTouchButton.SetActive(false);
        PlayerPrefs.SetInt("firsttime", 1);
    }

    // checks index number for whale.
    public void FishIndex()
    {
        for (int i = 0; i < fish.Length; i++)
        {
            if (fish[i].tag.Contains("whale"))
            {
                whaleIndex = i;
            }
        }
    }

    // starts next stage after short delay.
    IEnumerator NextStage(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }

    // Mutes audio source
    public void Mute()
    {
        Splash.volume = 0;
        Drop.volume = 0;
        Clear.volume = 0;
        buttonMute.SetActive(false);
        buttonUnmute.SetActive(true);
        PlayerPrefs.SetInt("sound", 0);
    }

    // UnMutes audio source
    public void UnMute()
    {
        Splash.volume = 1f;
        Drop.volume = 1f;
        Clear.volume = 1f;
        buttonMute.SetActive(true);
        buttonUnmute.SetActive(false);
        PlayerPrefs.SetInt("sound", 1);
    }
}