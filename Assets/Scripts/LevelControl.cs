using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public static LevelControl instance;

    public bool stageContinued;
    public bool boss;

    private int stage = 1;
    private int score = 0;
    private int bossFrequency =4;

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
        DontDestroyOnLoad(this);
    }

    // called from gamecontrol IncrementObjective function to increment stage and keep track of what stage we are at.
    public void IncrementStage()
    {
        stage++;
        stageContinued = true;

        // boss spawns in every "bossFrequency" stages.
        if (stage % bossFrequency == 0)
        {
            boss = true;
        }
        else
        {
            boss = false;
        }
    }

    // called from gamecontrol Start function to update our stage text when game starts.
    public void IncrementStageText()
    {
        GameControl.instance.TextCurrentStage.text = "Stage " + stage;

        // check to see if highstage playerpref is smaller then current stage. If it is then update highstage.
        if (PlayerPrefs.GetInt("highstage", 1) < stage)
        {
            PlayerPrefs.SetInt("highstage", stage);
        }
    }

    // called from gamecontrol Deathtimer function when gameover, to reset stage back to 1.
    public void StageReset()
    {
        stage = 1;
        score = 0;
        stageContinued = false;
        boss = false;
    }

    // called from gamecontrol IncrementObjective function to increment score.
    public void IncrementScore()
    {
        score += 1;
        IncrementScoreText();
    }

    // called from gamecontrol Start function to update our score text when game starts.
    public void IncrementScoreText()
    {
        GameControl.instance.TextCurrentScore.text = score.ToString();

        // check to see if highscore playerpref is smaller then current score. If it is then update highscore.
        if (PlayerPrefs.GetInt("highscore", 0) < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    // called from called from Gamecontrol Start function to reference what stage we are at.
    public int StageReturn()
    {
        return stage;
    }

    // called from called from Gamecontrol Start function .
    public int BossFreqReturn()
    {
        return bossFrequency;
    }
}
