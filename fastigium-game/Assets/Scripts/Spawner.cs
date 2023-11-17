using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject Platform;
    public bool objectExists = false;
    public string direction;
    public float speed;
    public float distanceX;
    public float distanceY;

    void Awake() {
        SetDistance();
    }

    void Update() {
        if(!objectExists) {
            GenerateObject();
        }
    }

    private void SetDistance() {
        transform.GetChild(0).transform.localPosition = new Vector3(distanceX, distanceY, 0);
    }

    public string GetDirection() {
        return direction;
    }

    public float GetSpeed() {
        return speed;
    }

    public void ObjectDestroyed() {
        objectExists = false;
    }

    private void GenerateObject() {
        Instantiate(Platform, transform.position, Quaternion.identity);
        this.objectExists = true;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.localPosition + new Vector3(distanceX, distanceY, 0), 0.5f);
    }
}
