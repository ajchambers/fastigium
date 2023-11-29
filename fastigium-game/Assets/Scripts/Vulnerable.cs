using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Vulnerable : MonoBehaviour {
    private Enemy1 e;
    public float bounceFactor = 10;

    private void Awake() {
        // get parent enemy1
        e = this.transform.parent.gameObject.GetComponent<Enemy1>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            e.Die();
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.JumpFromKill();
        }
    }
}