using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    public GameObject Platform;
    public bool objectExists;

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
