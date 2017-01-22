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
    public Text collectibleText;

	private float timerLevel;
	private int collectibleCount;
	private int collected;

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
//        if (timerLevel <= 0)
//        {
//            if(highScore < currentScore)
//            {
//                highScore = currentScore;
//                highScoreText.text = "Score: " + highScore;
//            }
//
//			if (!debug) 
//				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        }

		if (Input.GetButton("Reset")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

        UpdateUI();
    }

    public void Init() {
	    var obj = GameObject.Find("Score Text");
        scoreText = obj != null ? obj.GetComponent<Text>() : null;

		obj = GameObject.Find("Score Text");
		highScoreText = obj != null ? obj.GetComponent<Text>() : null;

        timerText = GameObject.Find("Timer Text").GetComponent<Text>();
        collectibleText = GameObject.Find("Collectible Text").GetComponent<Text>();

        currentScore = 0;

		collectibleCount = GameObject.FindGameObjectsWithTag("keycube").Length;

	    if (highScoreText != null) {
		    highScoreText.text = "Score: " + highScore;
	    }
    }

	public void Collect () {
		collected++;
	}

    public void IncreaseScore(int score)
    {
        currentScore += score;
        //Debug.Log(score);
    }

    public void UpdateUI()
    {
	    if (scoreText != null) {
		    scoreText.text = "Score: " + currentScore;
	    }

	    if (timerText != null) {
		    timerText.text = timerLevel.ToString("F1");
	    }

	    if (collectibleText != null) {
		    if (collectibleCount > 0) {
			    collectibleText.text = collected + "/" + collectibleCount;
		    }
		    else {
			    collectibleText.text = "";
		    }
	    }
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
