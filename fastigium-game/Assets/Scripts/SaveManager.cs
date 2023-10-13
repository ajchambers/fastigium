using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour, IManager {
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;

    private GameData gameData;
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
        // FindAllSaveableObjects();

    }

    public void PrintSaveableObjectsList() {
        Debug.Log("SaveManager's saveable objects:");
        foreach (ISaveable saveableObject in saveableObjects) {
            Debug.Log("     " + saveableObject);
        }
    }

    public void OnSceneUnloaded(Scene scene) {
        if ((scene.name != "MainMenu") || (scene.name != "OptionsMenu")) {
            // this.saveableObjects = FindAllSaveableObjects();
        }
    }

    // public void TakeDataFromSceneObjects() {
    //     this.saveableObjects = FindAllSaveableObjects();
    //     foreach (ISaveable saveableObject in saveableObjects) {
    //         saveableObject.SaveData(ref gameData);
    //     }
    // }

    public void PrepareSceneObjects() {
        this.saveableObjects = FindAllSaveableObjects();
        GiveDataToObjects(); // FOUND THE PROBLEM AREA
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

        // if there's no file, start a new one
        if (this.gameData == null){
            // Debug.Log("No game data found. Initializing data to defaults.");
            NewGame();
        }

        // load current scene
        SceneManager.LoadScene(gameData.currentScene); // TODO: problem area 

        // place a player at the spawn point
        GameObject p = Instantiate(player, gameData.playerPosition, Quaternion.identity);
        p.name = "Player";

        // prepare scene objects
        PrepareSceneObjects();

        // enable UICanvas
        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = true;
        pauseMenuUI.SetActive(false);
    }

    public void LoadLevel() {
        PrepareSceneObjects();
    }

    public void SaveGame() {
        Debug.Log(SceneManager.GetActiveScene().name);
        if ((SceneManager.GetActiveScene().name != "MainMenu") && (SceneManager.GetActiveScene().name != "OptionsMenu")) {
            // pass gameData to other scripts so they can update it
            foreach (ISaveable saveableObject in saveableObjects) { // TODO: problem area
                saveableObject.SaveData(ref gameData);
            }
            
            // save to file
            fileHandler.Save(gameData);
        }
    }

    private void OnApplicationQuit() {
            SaveGame();
    }

    private List<ISaveable> FindAllSaveableObjects() {
        IEnumerable<ISaveable> saveableObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();
        return new List<ISaveable>(saveableObjects);
    }
}