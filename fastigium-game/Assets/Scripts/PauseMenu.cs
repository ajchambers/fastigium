using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour{
    private UIManager umInstance;

    private void Awake() {
        umInstance = FindObjectOfType<UIManager>();
    }

    public void Resume() {
        umInstance.Resume();
    }

    public void SaveAndQuit () {
        umInstance.SaveAndQuit();
    }
}
