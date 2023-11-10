using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FallingPlatform : MonoBehaviour {
    public string id;
    public Spawner spawner;


    private void Awake() {
        GetClosestSpawner();    
    }

    public void GetClosestSpawner() {
        // get a list of all the spawners
        var spawners = FindObjectsOfType<Spawner>();

        // find the nearest one
        Spawner nearestSpawner = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(Spawner x in spawners) {
            Vector3 directionToTarget = x.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if(dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                nearestSpawner = x;
            }
        }

        // set this platform's spawner to it
        spawner = nearestSpawner;
    }

    private void OnDestroy() {
        spawner.objectDestroyed();
    }

}
