using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour, ISaveable {
    TimeManager tmInstance;
    [Header("Important Bools and More")]
    public bool isDead;
    public bool canMove = true;
    public bool hasFoundPlayer;
    public bool shootAndMoveActive;
    public bool canApproachPlayer = false;
    public bool noticeSoundPlayed = false;
    
    public float waitToMove;
    public float waitToShoot;
    
    public Vector2 target;

    [Header("For Saving")]
    [SerializeField] private string id;
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
    public float moveDirection = 1;
    public float initialYpos;

    public float distanceFromWall = 2;

    [Header("For Hovering")]
    public float hoverAmplitude;
    public float hoverSpeed;
    private float phaseOffset;

    [Header("For Attacking")]
    public float rayLength;
    private Transform player;
    public GameObject projectile;
    private Coroutine shootAndMoveRoutine;

    GeneralManager gm;
    AudioSource s;
 
    // set up general and time managers, rigidbody ref, hover phase offset, player ref
    void Awake() {
        gm = FindObjectOfType<GeneralManager>();
        enemyRB = GetComponent<Rigidbody2D>();
        initialYpos = transform.position.y;
        SetOffset();
        player = GameObject.Find("Player").transform;
        tmInstance = FindObjectOfType<TimeManager>();
        s = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        // check left and right for walls
        checkingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, circleRadius, groundLayer);
        checkingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, circleRadius, groundLayer);

        // determine states
        DetermineIfCanMove();
        CheckIfPlayerIsAlive();

        if (tmInstance.isTimeStopped) {
            s.mute = true;
        } else {
            s.mute = false;
        }

        // decide what to do based on those states
        if (canMove) {
            Hover();
            LookForPlayer();
            
            if (!hasFoundPlayer) {
                MoveHorizontally();

            } else if (hasFoundPlayer) {
                if (canApproachPlayer) {
                    MoveTowardsTarget();
                }
                if (!shootAndMoveActive) {
                    shootAndMoveRoutine = StartCoroutine(ShootAndMove());
                }
                Hover();
            }
        }

        if (transform.position.y < -12) {
            gameObject.SetActive(false);
        }
    }
 
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player")
            GeneralManager.gmInstance.KillPlayer();
      
        if (collision.gameObject.tag == "Missile")
            Die();
    }

    private void DetermineIfCanMove() {
        // if time is stopped, can't move
        if (tmInstance.isTimeStopped) {
            canMove = false;
            return;
        }

        // if enemy is dead, can't move
        if (isDead) {
            canMove = false;
            return;
        }

        canMove = true;
    }
    public void Die() {
        s.mute = true;
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        isDead = true;
        GetComponent<Obstacle>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<SpriteRenderer>().flipY = true;
        Vector3 movement = new Vector3(Random.Range(40, 70), Random.Range(-40, 40), 0f);
        transform.position += movement * Time.deltaTime;
        GetComponent<Rigidbody2D>().gravityScale = 4;
        gm.AddPoints(100);
        SaveManager.smInstance.SaveObject(this);
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

    private void SetOffset() {
        System.Random random = new System.Random();
        phaseOffset = random.Next(0, 100);
    }

    void Hover() {
        Vector2 p = transform.position;
        p.y = hoverAmplitude * Mathf.Cos(Time.time * hoverSpeed + phaseOffset) + initialYpos;
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
                if (!noticeSoundPlayed) {
                    FindObjectOfType<AudioManager>().Play("Enemy2NoticesPlayer");
                    noticeSoundPlayed = true;
                }
            }
        }
    }

    IEnumerator ShootAndMove() {
        shootAndMoveActive = true;
        enemyRB.velocity = Vector2.zero;
        Shoot();
        FindObjectOfType<AudioManager>().Play("Shoot");
        yield return new WaitForSeconds(waitToMove);

        canApproachPlayer = true;    
        SetTarget();

        yield return new WaitForSeconds(waitToShoot);
        
        canApproachPlayer = false;
        shootAndMoveActive = false;
    }

    void SetTarget() {
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 destination = new Vector2(player.position.x, initialYpos);

        string obstacleLayerName = "Ground";

        LayerMask obstacleLayer = LayerMask.GetMask(obstacleLayerName);

        // Check if there is an obstacle between the enemy and the player
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, destination - enemyPosition, Vector2.Distance(enemyPosition, destination), obstacleLayer);

        if (hit.collider == null) {
            target = new Vector2(player.position.x, initialYpos);

        } else {
            // Calculate a new destination point based on the hit point from the raycast

            float adjustment = Mathf.Sign(player.position.x - transform.position.x) * -1 * distanceFromWall;

            target = new Vector2(hit.point.x + adjustment, initialYpos);
        }
    }

    void MoveTowardsTarget() {
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(enemyPosition, target, 1.0f);
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
        shootAndMoveActive = false;
        noticeSoundPlayed = false;        

        if (shootAndMoveRoutine != null)
            StopCoroutine(shootAndMoveRoutine);
    }
}