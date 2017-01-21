using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore;
    public int highScore;

    public Text scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void IncreaseScore(int score)
    {
        currentScore += score;
        UpdateUI();
        Debug.Log(score);
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public int CalculateScoreBetweenNode(SinusWaveNode node1, SinusWaveNode node2)
    {
        //return 1;
        //Debug.Log(Vector3.Distance(node1.position, node2.position));
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
