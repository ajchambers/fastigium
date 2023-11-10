using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour, ISaveable {
    TimeManager tmInstance;

    [Header("For Saving")]
    [SerializeField] private string id;
    public bool isDead;
    private int skin;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    [Header("For Navigation")]
    [SerializeField] Transform wallCheckLeft;
    [SerializeField] Transform wallCheckRight;
    [SerializeField] float moveSpeed;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody2D enemyRB;
    public bool checkingWallLeft;
    public bool checkingWallRight;
    public bool facingRight = true;
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
 
    void Awake() {
        enemyRB = GetComponent<Rigidbody2D>();
        initialYpos = transform.position.y;
        player = GameObject.Find("Player").transform;
        tmInstance = FindObjectOfType<TimeManager>();
    }

    void FixedUpdate() {
        checkingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, circleRadius, groundLayer);
        checkingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, circleRadius, groundLayer);

        if (!tmInstance.isTimeStopped) {
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

    public void LoadData(GameData data) {
        data.enemy2data.TryGetValue(id, out EnemyData ed);

        if (ed != null) {
            if (ed.isDead) {
                gameObject.SetActive(false);
            }

            transform.position = ed.position;

            if (!ed.facingRight) {
                moveDirection *= -1;
            }

            hasFoundPlayer = ed.hasFoundPlayer;
        }

    }

    public void SaveData(ref GameData data) {
        EnemyData ed = new EnemyData(this.isDead, this.GetPosition(), this.initialYpos, this.facingRight, this.hasFoundPlayer, this.skin);
        
        if (data.enemy2data.ContainsKey(id)) {
            data.enemy2data.Remove(id);
        }

        data.enemy2data.Add(id, ed);
    }

    public Vector3 GetPosition() {
        return transform.position;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, 
            (1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Ground"))));
            
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
        if(!GeneralManager.gmInstance.isPlayerAlive) {
            Unlock();
        }
    }

    void Unlock() {
        hasFoundPlayer = false;
        hasMovedTowardPlayer = false;

        if (co != null)
            StopCoroutine(co);
    }

    void Die() {
        Destroy(gameObject);
    }

    bool TimeIsStopped() {
        return tmInstance.isTimeStopped;
    }
}
