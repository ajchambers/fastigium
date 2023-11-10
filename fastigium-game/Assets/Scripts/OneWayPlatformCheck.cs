using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformCheck : MonoBehaviour {
    Collider2D platformCollider;

    void Start() {
        platformCollider = gameObject.transform.parent.GetComponent<BoxCollider2D>();;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Physics2D.IgnoreCollision(other, platformCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Physics2D.IgnoreCollision(other, platformCollider, false);
    }

}
