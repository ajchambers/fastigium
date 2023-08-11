using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController scInstance;
    //public GameObject Player;

    //top
    Vector3 sp11 = new Vector3 (-10.5f, 7.0f);
    Vector3 sp12 = new Vector3 (0f, 7.0f);
    Vector3 sp1 = new Vector3 (10.5f, 7.0f);

    //right
    Vector3 sp2 = new Vector3 (17.25f, 6.0f);
    Vector3 sp3 = new Vector3 (17.25f, -1.0f);
    Vector3 sp4 = new Vector3 (17.25f, -9.0f);

    // bottom
    Vector3 sp5 = new Vector3 (12.5f, -9.0f);
    Vector3 sp6 = new Vector3 (2.5f, -9.0f);
    Vector3 sp7 = new Vector3 (-12.5f, -9.0f);

    // left
    Vector3 sp8 = new Vector3 (-17.25f, -9.0f);
    Vector3 sp9 = new Vector3 (-17.25f, -1.0f);
    Vector3 sp10 = new Vector3 (-17.25f, 6.0f);

    private void Awake() {
        if (scInstance == null) {
            scInstance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void NextLevel(string sceneName, Vector3 spawnPoint) {
        SceneManager.LoadScene(sceneName);
        GameObject.FindWithTag("Player").transform.position = spawnPoint;
    }
}
