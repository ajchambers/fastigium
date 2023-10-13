using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    // //top
    // Vector3 sp11 = new Vector3 (-10.5f, 7.0f);
    // Vector3 sp12 = new Vector3 (0f, 7.0f);
    // Vector3 sp1 = new Vector3 (10.5f, 7.0f);

    // //right
    // Vector3 sp2 = new Vector3 (17.25f, 6.0f);
    // Vector3 sp3 = new Vector3 (17.25f, -1.0f);
    // Vector3 sp4 = new Vector3 (17.25f, -9.0f);

    // // bottom
    // Vector3 sp5 = new Vector3 (12.5f, -9.0f);
    // Vector3 sp6 = new Vector3 (2.5f, -9.0f);
    // Vector3 sp7 = new Vector3 (-12.5f, -9.0f);

    // // left
    // Vector3 sp8 = new Vector3 (-17.25f, -9.0f);
    // Vector3 sp9 = new Vector3 (-17.25f, -1.0f);
    // Vector3 sp10 = new Vector3 (-17.25f, 6.0f);

    public string sceneName;
    public Vector3 spawnPoint;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            SceneController.scInstance.NextLevel(sceneName, spawnPoint);
        }
    }
}
