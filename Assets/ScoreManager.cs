using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore;
    public int highScore;
    public float timerLevel;

    public Text scoreText;
    public Text timerText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timerLevel -= Time.deltaTime;
        if (timerLevel <= 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }

        UpdateUI();
    }

    public void IncreaseScore(int score)
    {
        currentScore += score;
        Debug.Log(score);
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        timerText.text = "" + Mathf.Round(timerLevel);
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
