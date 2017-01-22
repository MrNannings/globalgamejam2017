using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClicked(Button button)
    {
        //Debug.Log(button.GetComponentInChildren<Text>().text);
        int levelNumber = int.Parse(button.GetComponentInChildren<Text>().text);
        if (levelNumber < 10)
        {
            Debug.Log("level00" + levelNumber);
            SceneManager.LoadScene("level00" + levelNumber);
        }
        else
        {
            Debug.Log("level0" + levelNumber);
            SceneManager.LoadScene("level0" + levelNumber);
        }
        
    }
}
