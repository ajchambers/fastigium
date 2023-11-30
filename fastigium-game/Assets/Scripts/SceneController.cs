using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, ISaveable {
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;
    public static DialogueManager dmInstance;

    public string currentScene;

    private void Start() {
        LookForManagers();
    }

    public void LookForManagers() {
        gmInstance = FindObjectOfType<GeneralManager>();
        scInstance = this;
        smInstance = FindObjectOfType<SaveManager>();
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = FindObjectOfType<UIManager>();
        dmInstance = FindObjectOfType<DialogueManager>();
    }

    public void NextLevel(string sceneName, Vector3 spawnPoint) {
        // save data in pre transition scene
        smInstance.SaveGame();

        // load the next scene
        SceneManager.LoadScene(sceneName);

        // update the current scene attribute
        SetCurrentScene(sceneName);

        // move player to the correct point in the next level
        GameObject.FindWithTag("Player").transform.position = spawnPoint;

        // if the dialogue box is on, turn it off
        dmInstance.EndDialogue();
    }

    public string GetCurrentScene() {
        return currentScene;
    }

    public void SetCurrentScene(string currentScene) {
        this.currentScene = currentScene;
    }

    public void LoadData(GameData data) {
        // this.currentScene = data.currentScene; // TODO: here
    }

    public void SaveData(ref GameData data) {
        data.currentScene = this.GetCurrentScene(); // TODO: here
    }
}