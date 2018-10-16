using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    
    private GameObject gameOverImage;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
	}

    void InitGame() {
        gameOverImage = GameObject.Find("GameOverImage");

        gameOverImage.SetActive(false);
    }
	
	public void GameOver() {
        gameOverImage.SetActive(true);
        enabled = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // do stuff
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
