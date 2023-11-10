using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleThroughPlatform : MonoBehaviour {
    BoxCollider2D col;
    CapsuleCollider2D pcol;
    bool touching = false;

    void Start() {
        col = GetComponent<BoxCollider2D>();
        pcol = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (touching) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Debug.Log("S press detected.");
                Physics2D.IgnoreCollision(pcol, col, true);

            } else if (Input.GetKeyUp(KeyCode.S)) {
                Physics2D.IgnoreCollision(pcol, col, true);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.tag == "Player") {
    //         touching = true;
    //     }        
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     touching = false;
    // }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            touching = true;
            Debug.Log("Touching player.");
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        touching = false;
        Debug.Log("Not touching player.");
    }
}
