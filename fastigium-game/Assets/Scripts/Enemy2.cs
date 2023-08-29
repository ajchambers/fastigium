using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {
    [Header("For Navigation")]
    [SerializeField] Transform wallCheckLeft;
    [SerializeField] Transform wallCheckRight;
    [SerializeField] float moveSpeed;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody2D enemyRB;
    public bool checkingWallLeft;
    public bool checkingWallRight;
    public bool facingRight;
    private float moveDirection = 1;
    private float initialYpos;

    [Header("For Hovering")]
    public float hoverAmplitude;
    public float hoverSpeed;

    [Header("For Attacking")]
    public float rayLength;
    public bool hasFoundPlayer;
    public bool hasMovedTowardPlayer;
    private Transform player;
    public float pauseDuration;
    public float lockSpeed;
    public GameObject projectile;
    private Coroutine co;
 
    void Start() {
        enemyRB = GetComponent<Rigidbody2D>();
        initialYpos = transform.position.y;
        player = GameObject.Find("Player").transform;
    }

    void FixedUpdate() {
        checkingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, circleRadius, groundLayer);
        checkingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, circleRadius, groundLayer);

        if (!TimeIsStopped()) {
            Hover();
            LookForPlayer();

            if (!hasFoundPlayer) {
                MoveHorizontally();
            } else if (hasFoundPlayer) {
                if (!hasMovedTowardPlayer) {
                    co = StartCoroutine(LockOnPlayer());
                }
            }
            CheckIfPlayerIsAlive();
        }
    }

    void Hover() {
        Vector2 p = transform.position;
        p.y = hoverAmplitude * Mathf.Cos(Time.time * hoverSpeed) + initialYpos;
        transform.position = p;
    }

    void MoveHorizontally() {
        if (checkingWallLeft || checkingWallRight) {
            Flip();
        }
        enemyRB.velocity = new Vector2(moveSpeed * moveDirection, enemyRB.velocity.y);       
    }

    void Flip() {
        facingRight = !facingRight;
        moveDirection *= -1;
    }

    void LookForPlayer() { 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, (1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Ground"))));
        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red, 0);
        
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == "Player") {
                hasFoundPlayer = true;
            }
        }
    }

    IEnumerator LockOnPlayer() {
        hasMovedTowardPlayer = true;
        enemyRB.velocity = Vector2.zero;
        Shoot();
        yield return new WaitForSeconds(pauseDuration);

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer() {
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 destination = new Vector2(player.position.x, initialYpos);
        float step = lockSpeed * Time.deltaTime * 1000;
        transform.position = Vector2.MoveTowards(enemyPosition, destination, step);
        hasMovedTowardPlayer = false;
    }

     private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Missile")) {
            Die();
        }
    }

    void Shoot() {
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y - 1.15f);
        Quaternion q = new Quaternion(0, 0, -180, 0);
        Instantiate(projectile, (Vector3)enemyPosition, q);
    }

    void CheckIfPlayerIsAlive() {
        if(!GameManager.gmInstance.isPlayerAlive) {
            Unlock();
        }
    }

    void Unlock() {
        hasFoundPlayer = false;
        hasMovedTowardPlayer = false;
        StopCoroutine(co);
    }

    void Die() {
        Destroy(gameObject);
    }

    bool TimeIsStopped() {
        return GameManager.gmInstance.GetComponent<TimeManager>().isTimeStopped;
    }
}
