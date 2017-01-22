using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

	public bool debug = true;
    public int currentScore;
    public int highScore;
    public float timePerUnit = 5;

    public Text scoreText;
    public Text timerText;
    public Text highScoreText;

	private float timerLevel;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        Init();
    }

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        timerLevel += Time.deltaTime;
        if (timerLevel <= 0)
        {
            if(highScore < currentScore)
            {
                highScore = currentScore;
                highScoreText.text = "Score: " + highScore;
            }

			if (!debug) 
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

		if (Input.GetButton("Reset")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

        UpdateUI();
    }

    public void Init()
    {
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScore Text").GetComponent<Text>();

        currentScore = 0;
        highScoreText.text = "Score: " + highScore;
    }

    public void IncreaseScore(int score)
    {
        currentScore += score;
        //Debug.Log(score);
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        timerText.text = timerLevel.ToString("F1");
    }

    public int CalculateScoreBetweenNode(SinusWaveNode node1, SinusWaveNode node2)
    {
        return Mathf.RoundToInt(Vector3.Distance(node1.position, node2.position) * 20);
    }

    public void CalculateAllNodesScore(List<SinusWaveNode> sinusWaveNodesList)
    {
        int score = 0 ;
        foreach (SinusWaveNode node in sinusWaveNodesList)
        {
            score += node.score;
        }
    }
}
