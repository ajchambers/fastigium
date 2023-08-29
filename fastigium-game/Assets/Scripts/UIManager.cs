using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public static UIManager uiManagerInstance;

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Awake() {
        if (uiManagerInstance == null) {
            uiManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    // TODO: this...
    public void Save() {
        Debug.Log("Game saved.");
    }

    public void LoadMenu() {
        pauseMenuUI.SetActive(false);

        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("UICanvas"));
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
