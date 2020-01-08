using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public UIManager UI;
    
    private GameObject gameOverImage;
    private GameObject pauseMenuPanel;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        InitGame();
	}

    void InitGame() {
        gameOverImage = GameObject.Find("GameOverImage");

        gameOverImage.SetActive(false);

        pauseMenuPanel = GameObject.Find("PauseMenuPanel");

        pauseMenuPanel.SetActive(false);
    }
	
	public void GameOver() {
        Time.timeScale = 0f;
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

    public void TogglePauseMenu() {
        // not the optimal way but for the sake of readability
        if (pauseMenuPanel.activeSelf) {
            pauseMenuPanel.SetActive(false);
            foreach (Transform t in pauseMenuPanel.transform) {
                if (t.GetComponent<Button>()) {
                    t.GetComponent<Button>().interactable = false;
                }
            }
            Time.timeScale = 1.0f;
        } else {
            pauseMenuPanel.SetActive(true);
            foreach (Transform t in pauseMenuPanel.transform) {
                if (t.GetComponent<Button>()) {
                    t.GetComponent<Button>().interactable = true;
                }
            }
            Time.timeScale = 0f;
        }

        Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
    }

    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
