using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class MainMenu : MonoBehaviour {

    private void Awake() {
        // disable the game manager's child, UICanvas's canvas
        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = false;
    }

    public void Play() {
        GeneralManager.gmInstance.GetComponent<SaveManager>().LoadGame();
    }

    public void ClearSaveData(){
        string fullPath = Path.Combine(Application.persistentDataPath, "save.txt");
        try {
            File.Delete(fullPath);
        } catch (Exception e) {
            Debug.Log("Couldn't delete file at " + fullPath);
            Debug.Log(e);
        }
        Debug.Log("Save data cleared.");
    }

    public void Options() {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void Quit() {
        Application.Quit();
    }
} 