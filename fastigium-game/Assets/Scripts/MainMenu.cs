using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public GameObject popupMenu;
    public TextMeshProUGUI popupMenuText;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject okButton;

    private void Awake() {
        // disable the game manager's child, UICanvas's canvas
        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = false;
    }

    public void Play() {
        SaveManager.smInstance.ResumeGame();
    }

    public void TriggerPopup() {
        popupMenu.SetActive(true);
        popupMenuText.SetText("are you sure you want to clear your save data?");

        yesButton.SetActive(true);
        noButton.SetActive(true);

        okButton.SetActive(false);
    }

    public void ClosePopup() {
        popupMenu.SetActive(false);
    }

    public void DontClearSaveData(){
        popupMenu.SetActive(false);
    }

    public void ClearSaveData(){
        string fullPath = Path.Combine(Application.persistentDataPath, "save.txt");

        try {
            File.Delete(fullPath);
        } catch (Exception e) {
            Debug.Log("Couldn't delete file at " + fullPath);
            Debug.Log(e);
        }
        
        popupMenuText.SetText("save data cleared.");

        yesButton.SetActive(false);
        noButton.SetActive(false);

        okButton.SetActive(true);
    }

    public void Options() {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void Quit() {
        Application.Quit();
    }
} 