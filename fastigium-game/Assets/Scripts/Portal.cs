using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public string nextSceneName;
    public Vector3 spawnPoint;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            SceneController.scInstance.NextLevel(nextSceneName, spawnPoint);
        }
    }
}
