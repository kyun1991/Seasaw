using System.Collections;
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

    public bool noMoreObjective;

    private bool startTimer;   
    private int bossCounter;
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

        objectiveNumber = LevelControl.instance.StageReturn();
        StageControl(objectiveNumber);
        int whatStage = LevelControl.instance.StageReturn()%LevelControl.instance.BossFreqReturn();
        for (int i = 0; i < whatStage; i++)
        {
            stageIndicator[i].color = new Color32(226, 87, 76, 255);
        }
        LevelControl.instance.IncrementStageText();
        LevelControl.instance.IncrementScoreText();
        
        TextHighStageAndScore.text = "Stage " + PlayerPrefs.GetInt("highstage", 1) + " , " + PlayerPrefs.GetInt("highscore", 0);

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


        // if boss stage, then initialise boss attack depending on what boss it is.
        if (LevelControl.instance.boss == true)
        {
            for (int i = 0; i < stageIndicator.Length; i++)
            {
                stageIndicator[i].color = new Color32(226, 87, 76, 255);
            }            
            bossCounter = LevelControl.instance.StageReturn()/ LevelControl.instance.BossFreqReturn();

            if (bossCounter == 1)
            {
                StartCoroutine(GetComponent<BossControl>().BossOne());
            }
            else if (bossCounter == 2)
            {
                StartCoroutine(GetComponent<BossControl>().BossTwo());
            }
            else if (bossCounter == 3)
            {
                StartCoroutine(GetComponent<BossControl>().BossThree());
            }
            else if (bossCounter == 4)
            {
                StartCoroutine(GetComponent<BossControl>().BossFour());
            }
            else if (bossCounter == 5)
            {
                StartCoroutine(GetComponent<BossControl>().BossFive());
            }
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
        spawned[0].transform.position = new Vector2(0, spawnHeight);
        for (int i = 0; i < preview.Length; i++)
        {
            if (spawned[1].tag == preview[i].tag)
            {
                preview[i].SetActive(true);
            }
        }
        TextObjectiveNumber.text = objectiveNumber.ToString();
        stageIndicatorGO.SetActive(true);
    }

    // returns list of spawned objectives. Used in Touch script.
    public List<GameObject> Spawned()
    {
        return spawned;
    }

    // changing difficulty of stages.
    public void StageControl(int stage)
    {
       // objectiveNumber = stage + 4;

        if (stage== 1)
        {
            objectiveNumber = 3;
        }

        if (stage < 5)
        {
            objectiveNumber = 3;
        }
    }
}