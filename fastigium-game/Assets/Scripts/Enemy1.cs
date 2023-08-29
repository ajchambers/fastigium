using UnityEngine;
public class Enemy1 : MonoBehaviour {
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

    [Header("For SeeingPlayer")]
    [SerializeField] Vector2 lineOfSite;
    [SerializeField] LayerMask playerLayer;
    public bool canSeePlayer;

    [Header("For Time Mechanic")]
    public bool isFrozen;

    [Header("Other")]
    // private Animator enemyAnim;
    private Rigidbody2D enemyRB;

    void Start() {
        enemyRB = GetComponent<Rigidbody2D>(); 
        Transform player = GameObject.Find("Player").transform;

        // enemyAnim = GetComponent<Animator>();        
    }

    void FixedUpdate() {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        
        // AnimationController();

        // if time is not stopped
        if (!GameManager.gmInstance.GetComponent<TimeManager>().isTimeStopped) {
            if (canSeePlayer && isGrounded) {
                JumpAttack();
            }
            if (!canSeePlayer && isGrounded) {
                Petrolling();
            }   
        }
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
