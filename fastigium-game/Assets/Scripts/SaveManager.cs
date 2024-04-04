using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using TarodevController;

public class SaveManager : MonoBehaviour, IManager {
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;

    public GameData gameData;
    private List<ISaveable> saveableObjects;
    private DataFileHandler fileHandler;

    [Header("File Name")]
    [SerializeField] private string fileName;

    [SerializeField] GameObject player;

    // TODO: this should be handled by the UIManager
    public GameObject pauseMenuUI;

    private void Awake() {
        this.fileHandler = new DataFileHandler(Application.persistentDataPath, fileName);
    }

    private void Start() {
        LookForManagers();
    }

    public void LookForManagers() {
        gmInstance = FindObjectOfType<GeneralManager>();
        scInstance = FindObjectOfType<SceneController>();
        smInstance = this;
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = FindObjectOfType<UIManager>();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if ((scene.name != "MainMenu") && (scene.name != "OptionsMenu")) {
            saveableObjects = FindAllSaveableObjects();
            GiveDataToObjects();
            scInstance.SetCurrentScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnSceneUnloaded(Scene scene) {}

    public void PrintSaveableObjectsList() {
        Debug.Log("SaveManager's saveable objects:");
        foreach (ISaveable saveableObject in saveableObjects) {
            Debug.Log("\n   " + saveableObject);
        }
    }

    private List<ISaveable> FindAllSaveableObjects() {
        IEnumerable<ISaveable> saveableObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();
        return new List<ISaveable>(saveableObjects);
    }

    public void GiveDataToObjects() {
        // push the loaded data to all the scripts that need it
        foreach (ISaveable saveableObject in saveableObjects) {
            saveableObject.LoadData(gameData);
        }
    }

    public void NewGame() {
        this.gameData = new GameData();
    }

    public void ResumeGame() {
        // load save file
        this.gameData = fileHandler.Load();

        // if there's a file that shows the player has finished the game...
        if (this.gameData.hasFinishedGame) {            
            // ...delete the previous file.
            string fullPath = Path.Combine(Application.persistentDataPath, "save.txt");
            try {
                File.Delete(fullPath);
            } catch (Exception e) {
                Debug.Log("Couldn't delete file at " + fullPath);
                Debug.Log(e);
            }

            this.gameData = null;
        }

        // if there's no file, start a new one
        if (this.gameData == null) {
            NewGame();
            Debug.Log("No save file detected. Starting a new file.");
        }

        // load current scene
        SceneManager.LoadScene(gameData.currentScene);

        // place a player at the spawn point
        GameObject p = Instantiate(player, gameData.playerPosition, Quaternion.identity);
        p.name = "Player";
        
        // if there was a preexisting player, move it to the spawn point
        PlayerController.playerInstance.transform.position = gameData.playerPosition;
        
        // set list of Saveable objects
        this.saveableObjects = FindAllSaveableObjects();

        // give data to those objects
        GiveDataToObjects();

        // enable UICanvas
        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = true;
        pauseMenuUI.SetActive(false);
    }

    public bool CanSave() {
        if ((SceneManager.GetActiveScene().name == "MainMenu"))
            return false;

        if ((SceneManager.GetActiveScene().name == "OptionsMenu"))
            return false;

        // if ((SceneManager.GetActiveScene().name == "EndCutscene"))
        //     return false;

        if ((SceneManager.GetActiveScene().name == "ScoreScreen"))
            return false;

        if (!gmInstance.isPlayerAlive)
            return false;

        return true;
    }

    public void SaveGame() {
        if (CanSave()) {

            // update list of objects to save
            this.saveableObjects = FindAllSaveableObjects();

            // pass gameData to other scripts so they can update it
            foreach (ISaveable saveableObject in saveableObjects) { // TODO: problem area
                saveableObject.SaveData(ref gameData);
            }
            
            // save to file
            fileHandler.Save(gameData);
        }
    }

    public void SaveObject(ISaveable g) {
        g.SaveData(ref gameData);
        fileHandler.Save(gameData);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }
}