using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour {
    public GameObject nextButton;

    private void Awake() {
        // mark game save as completed
        SaveManager.smInstance.gameData.hasFinishedGame = true;
        SaveManager.smInstance.SaveGame();
        LoadCutscene();
    }
    public void LoadCutscene() {
        // hide UI and player
        

        // display cutscene image


        // gently truck camera right until it hits a certain point


        // wait, and then enable "next button"
        
    }

    public void GoToStats() {
        SceneManager.LoadScene("ScoreScreen");
    }
}
