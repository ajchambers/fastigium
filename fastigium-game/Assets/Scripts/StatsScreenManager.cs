using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsScreenManager : MonoBehaviour {
    public Text scoreText;
    public Text deathText;

    private void Awake() {
        ShowScore(SaveManager.smInstance.gameData.score);
        ShowDeaths(SaveManager.smInstance.gameData.deathCount);
    }
    public void Back() {
        SceneController.scInstance.currentScene = "MainMenu";
        SceneController.scInstance.previousScene = "OptionsMenu";
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowScore(int score) {
        scoreText.text = score.ToString();
    }

    public void ShowDeaths(int deaths) {
        deathText.text = deaths.ToString();
    }

    public void QuitToMenu() {
        SceneController.scInstance.currentScene = "MainMenu";
        SceneController.scInstance.previousScene = "OptionsMenu";
        SceneManager.LoadScene("MainMenu");
    }
}
