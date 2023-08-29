using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Play() {
        SceneManager.LoadScene("t1");
    }

    public void ClearSaveData() {
        Debug.Log("Are you sure you want to clear all progess?");
    }


    public void Options() {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}
