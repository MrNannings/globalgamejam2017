using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void LoadNextLevel () {
		var name = SceneManager.GetActiveScene().name;

		name = name.Remove(0, "level".Length);

		while (name.Length > 1 && name.StartsWith("0")) {
			name = name.Remove(0, 1);
		}

		int levelNumber;
		if (!int.TryParse(name, out levelNumber)) {
			SceneManager.LoadScene("MainMenu");
			return;
		}

		var zeros = 0;
		if (levelNumber < 10) {
			zeros = 2;
		}
		else if (levelNumber < 100) {
			zeros = 1;
		}

		var levelName = "level";
		for (int i = 0; i < zeros; i++) {
			levelName += "0";
		}
		levelName += levelNumber + 1;
		
		if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
			SceneManager.LoadScene("MainMenu");
			return;
		}

		SceneManager.LoadScene(levelName);
	}

	public void ContinueLastLevel () {
        SceneManager.LoadScene("level001");
	}

    public void LevelSelectScreen()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Quit () {
		Application.Quit();
	}
}
