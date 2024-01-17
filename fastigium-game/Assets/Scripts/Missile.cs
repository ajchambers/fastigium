using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    TimeManager tmInstance;
    public Transform target;

    private Rigidbody2D rb;
    private PolygonCollider2D c;

    public float travelSpeed;
    public float rotateSpeed;
    AudioSource s;

    void Start() {
        tmInstance = FindObjectOfType<TimeManager>();
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<PolygonCollider2D>();
        s = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        if (!tmInstance.isTimeStopped) {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;        
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * travelSpeed;
            s.mute = false;
        } else {
            rb.angularVelocity = 0;
            s.mute = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject g = collision.gameObject;

        if(g.name == "Player") {
            GeneralManager.gmInstance.KillPlayer();
        }

        if (g.GetComponent<Enemy2>() != null) {
            Enemy2 e = g.GetComponent<Enemy2>();
            e.Die();
        }
        Explode();
    }

    void Explode() {
        FindObjectOfType<AudioManager>().Play("MissileExplosion");
        Destroy(gameObject);
    }
}