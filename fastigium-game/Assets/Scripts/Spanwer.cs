using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    public GameObject Platform;
    public bool objectExists;

    void Update() {
        generateObject();
    }

    public void objectDestroyed() {
        objectExists = false;
    }

    private void generateObject() {
        if(!objectExists) {
            Instantiate(Platform, transform.position, Quaternion.identity);
            this.objectExists = true;
        }
    }
}
