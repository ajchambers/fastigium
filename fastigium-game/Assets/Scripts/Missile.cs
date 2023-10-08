using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    public Transform target;

    private Rigidbody2D rb;
    private PolygonCollider2D c;

    public float travelSpeed;
    public float rotateSpeed;

    public Enemy2 enemy2;

    void Start() {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate() {
        if (!GeneralManager.gmInstance.GetComponent<TimeManager>().isTimeStopped) {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;        
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * travelSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.name == "Player") {
            GeneralManager.gmInstance.KillPlayer();
        }
        Explode();
    }

    void Explode() {
        Destroy(gameObject);
    }
}