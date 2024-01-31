using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualsManager : MonoBehaviour {
    public Color p1Color = Color.blue;
    public Color p2Color = Color.yellow;
    public Color p3Color = Color.red;

    public Camera cam;
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start() {
        
    }

    void Update() {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        UpdateBackgroundColor();
        UpdateCamera();
    }

    private void UpdateBackgroundColor() {
        string name = SceneManager.GetActiveScene().name.Substring(0, 2);

        switch(name) {
            case "p1":
                // Debug.Log("Changing background color to " + p1Color);
                SetBackgroundColor(p1Color);
                break;
            case "p2":
                SetBackgroundColor(p2Color);
                break;
            case "p3":
                SetBackgroundColor(p3Color);
                break;
            default:
                // Debug.Log("Scene name isn't p1, p2, or p3");
                break;
        }        
    }

    private void SetBackgroundColor(Color c) {
        if (cam != null) {
            cam.backgroundColor = c;
            // Debug.Log("Set background color to " + c);
        } else {
            UpdateCamera();
            cam.backgroundColor = c;
            // Debug.Log("Set background color to " + c);
        }
    }

    private void UpdateCamera() {
        cam = FindObjectOfType<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}
