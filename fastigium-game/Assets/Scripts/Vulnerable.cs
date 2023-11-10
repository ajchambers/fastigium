using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulnerable : MonoBehaviour {
    private Enemy1 e;

    private void Awake() {
        // get parent enemy1
        e = this.transform.parent.gameObject.GetComponent<Enemy1>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            e.Die();
        }
    }
}