using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveForcer : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Save();
            Debug.Log(this + " has forced a save.");
        }
    }

    private void Save() {
        SaveManager.smInstance.SaveGame();
    }
}
