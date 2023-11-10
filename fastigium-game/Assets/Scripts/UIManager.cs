using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManager {
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;

    // flags
    public static bool gameIsPaused = false;
    
    public GameObject pauseMenuUI;

    private void Awake() {
        LookForManagers();
        Canvas canvas =  GameObject.Find("UICanvas").GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
        pauseMenuUI.SetActive(false);
    }

    public void LookForManagers() {
        gmInstance = FindObjectOfType<GeneralManager>();
        scInstance = FindObjectOfType<SceneController>();
        smInstance = FindObjectOfType<SaveManager>();
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = this;
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
        //smInstance.PrintSaveableObjectsList();
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void SaveAndQuit() {
        // unpause the game
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        // save game
        smInstance.SaveGame();
        
        //(GameObject.Find("GeneralManager")).SetActive(false);

        Destroy(GameObject.Find("Player"));
        // (GameObject.Find("UICanvas")).SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
