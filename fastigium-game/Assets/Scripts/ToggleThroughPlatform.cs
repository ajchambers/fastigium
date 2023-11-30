using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleThroughPlatform : MonoBehaviour {
    BoxCollider2D col;
    CapsuleCollider2D pcol;
    CapsuleCollider2D icol;
    bool touching = false;

    void Start() {
        col = GetComponent<BoxCollider2D>();
        pcol = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (touching) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Physics2D.IgnoreCollision(pcol, col, true);

            } else if (Input.GetKeyUp(KeyCode.S)) {
                Physics2D.IgnoreCollision(pcol, col, true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            touching = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        touching = false;
    }
}
