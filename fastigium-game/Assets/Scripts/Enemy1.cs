using UnityEngine;

public class Enemy1 : MonoBehaviour, ISaveable {
    [Header("For Saving")]
    [SerializeField] private string id;
    public bool isDead;
    public Vector3 position;
    private int skin;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }
    
    [Header("For Patrolling")]
    [SerializeField] float moveSpeed;
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float circleRadius;
    public bool checkingGround;
    public bool checkingWall;

    [Header("For JumpAttacking")]
    [SerializeField] float jumpHeight;
    public Transform player;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    public bool isGrounded;

    [Header("For Dying")]
    [SerializeField] CircleCollider2D brain;

    [Header("For SeeingPlayer")]
    [SerializeField] Vector2 lineOfSite;
    [SerializeField] LayerMask playerLayer;
    public bool canSeePlayer;

    [Header("For Time Mechanic")]
    public bool isFrozen;

    [Header("Other")]
    // private Animator enemyAnim;
    private Rigidbody2D enemyRB;

    private void Awake() {
        enemyRB = GetComponent<Rigidbody2D>();
        // enemyAnim = GetComponent<Animator>(); 
    }

    private void Start() {
        Transform player = GameObject.Find("Player").transform;
    }

    void FixedUpdate() {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        
        // AnimationController();

        // if time is not stopped
        if (!TimeManager.tmInstance.isTimeStopped) {
            if (canSeePlayer && isGrounded) {
                JumpAttack();
            }
            if (!canSeePlayer && isGrounded) {
                Petrolling();
            }   
        }
    }

    public void LoadData(GameData data) {
        data.enemy1data.TryGetValue(id, out EnemyData ed);

        if (ed != null) {
            if (ed.isDead) {
                gameObject.SetActive(false);
            }

            transform.position = ed.position;

            if (!ed.facingRight) {
                Flip();
            }
        }
    }

    public void SaveData(ref GameData data) {
        EnemyData ed = new EnemyData(this.isDead, this.GetPosition(), this.facingRight, this.skin);
        
        if (data.enemy1data.ContainsKey(id)) {
            data.enemy1data.Remove(id);
        }

        data.enemy1data.Add(id, ed);
    }

    public void Die() {
        gameObject.SetActive(false);
        MarkAsDead();
    }

    public void MarkAsDead() {
        isDead = true;
        SaveManager.smInstance.SaveObject(this);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    void Petrolling() {
        if (!checkingGround || checkingWall) {
            if (facingRight) {
                Flip();
            } else if (!facingRight) {
                Flip();
            }
        }
        enemyRB.velocity = new Vector2(moveSpeed * moveDirection, enemyRB.velocity.y);
    }

    void JumpAttack() {
        player = GameObject.Find("Player").transform;

        // static jump force?
        float distanceFromPlayer = player.position.x - transform.position.x;

        if (isGrounded) {
            enemyRB.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
    }

    void FlipTowardsPlayer() {
        float playerPosition = player.position.x - transform.position.x;
        if (playerPosition < 0 && facingRight) {
            Flip();
        } else if (playerPosition > 0 && !facingRight) {
            Flip();
        }
    }

    void Flip() {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // void AnimationController() {
    //     enemyAnim.SetBool("canSeePlayer", canSeePlayer);
    //     enemyAnim.SetBool("isGrounded", isGrounded);
    // }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    } 
}
