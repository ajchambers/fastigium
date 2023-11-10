using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner : MonoBehaviour {
    public GameObject Platform;
    public bool objectExists = false;

    void Update() {
        GenerateObject();
    }

    public void objectDestroyed() {
        objectExists = false;
    }

    private void GenerateObject() {
        if(!objectExists) {
            Instantiate(Platform, transform.position, Quaternion.identity);
            this.objectExists = true;
        }
    }
}
