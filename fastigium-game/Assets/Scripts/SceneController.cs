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

    public string currentScene;

    //public GameObject Player;

    private void Start() {
        LookForManagers();
    }

    public void LookForManagers() {
        gmInstance = FindObjectOfType<GeneralManager>();
        scInstance = this;
        smInstance = FindObjectOfType<SaveManager>();
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = FindObjectOfType<UIManager>();
    }

    public void NextLevel(string sceneName, Vector3 spawnPoint) {
        // save data in pre transition scene
        smInstance.SaveGame();

        // load the next scene
        SceneManager.LoadScene(sceneName);

        // update the current scene attribute
        SetCurrentScene(sceneName);

        // give data to objects
        // smInstance.PrepareSceneObjects();
        // Debug.Log("SceneController called smInstance.PrepareSceneObjects");

        // save the current scene
        // smInstance.SaveGame();

        // Debug.Log("SceneController's current scene name: " + this.currentScene);

        // move player to the correct point in the next level
        GameObject.FindWithTag("Player").transform.position = spawnPoint;
    }

    public string GetCurrentScene() {
        return currentScene;
    }

    public void SetCurrentScene(string currentScene) {
        this.currentScene = currentScene;
    }

    public void LoadData(GameData data) {
        this.currentScene = data.currentScene; // TODO: here
    }

    public void SaveData(ref GameData data) {
        data.currentScene = this.GetCurrentScene(); // TODO: here
    }
}