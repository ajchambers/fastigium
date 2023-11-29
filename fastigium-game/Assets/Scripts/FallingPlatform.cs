using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingPlatform : MonoBehaviour {
    public Spawner spawner;
    public string direction;
    public float speed;

    private void Awake() {
        GetClosestSpawner();
        SetDirection();
        SetSpeed();
    }

    private void Update() {
        if (!TimeManager.tmInstance.isTimeStopped) {
            switch (direction) {
                case "up":
                    MoveUp();
                    break;
                case "down":
                    MoveDown();
                    break;
                case "left":
                    MoveLeft();
                    break;
                case "right":
                    MoveRight();
                    break;
            }
        }
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void MoveUp() {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void MoveDown() {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void MoveLeft() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void MoveRight() {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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

    private void SetDirection() {
        direction = spawner.GetDirection();
    }

    private void SetSpeed() {
        speed = spawner.GetSpeed();
    }

    private void OnDestroy() {
        spawner.ObjectDestroyed();
    }

}
