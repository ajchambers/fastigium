using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TimeBody : MonoBehaviour
{
    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timeManager;
    private Rigidbody2D rb;
    private Vector2 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;

    bool isRewinding = false;
    public float recordTime = 5f;
    List<PointInTime> pointsInTime;

    public SpriteRenderer sr;
    public Color defaultColor;
    public Color hoverColor;
    public Color selectedColor;
    public Color recallColor;
    
    bool isSelected;
    bool isHovered;

    private Vector3 lastPosition;
    private Transform t;
    private bool isMoving;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        timeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeManager>();
        TimeBeforeAffectedTimer = TimeBeforeAffected;

        t = transform;
        lastPosition = t.position;
        isMoving = false;

        pointsInTime = new List<PointInTime>();
    }

    void Update() { 
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if(TimeBeforeAffectedTimer <= 0f) {
            CanBeAffected = true; // Will be affected by timestop
        }

        if(CanBeAffected && timeManager.isTimeStopped && !IsStopped){
            StopTime();
        }

        if (Input.GetMouseButtonDown(0) && IsStopped && isSelected) {
            StartRewind();
        }

        if (Input.GetMouseButtonUp(0)) {
            isSelected = false;
            StopRewind();
        }
    }

   	void FixedUpdate () {
        handleBodyColor();
        checkIfObjectMoving();
		if (isRewinding) {
			Rewind();
        } else {
			Record();
        }
	}

    // stick the player to the platform
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            collision.transform.SetParent(transform);
    }

    // unstick the player to the platform
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            collision.transform.SetParent(null);
    }

    public void StopTime() {
        if(rb.velocity.magnitude >= 0f) { //If Object is moving
            recordedVelocity = rb.velocity.normalized; //records direction of movement
            recordedMagnitude = rb.velocity.magnitude; // records magitude of movement

            rb.velocity = Vector2.zero; //makes the rigidbody stop moving
            rb.isKinematic = true; //not affected by forces
            IsStopped = true; // prevents this from looping
        }
    }

    public void ContinueTime() {
        rb.isKinematic = false;
        IsStopped = false;
        // rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the recorded velocity when time continues
    }

 	public void StartRewind () {
		isRewinding = true;
		rb.isKinematic = true;
	}

	public void StopRewind () {
		isRewinding = false;
		// rb.isKinematic = false;
	}   

 	void Rewind () {
		if (pointsInTime.Count > 0) {
			PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveAt(0);
		} else {
			StopRewind();
		}
	} 

	void Record () {
            // removes the old positions in pointsInTime list when recordTime has passed
            if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime)) {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            // record if the object is moving
            if (isMoving) {
                pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
            }
	}

    void checkIfObjectMoving() {
        if (t.position != lastPosition) {
            isMoving = true;
        } else {
            isMoving = false;
        }

        lastPosition = t.position;
    }

    private void OnMouseEnter() {
        isHovered = true;
    }

    private void OnMouseExit() {
        isHovered = false;
    }

    private void OnMouseDown() {
        isHovered = false;
        isSelected = true;
    }

    void handleBodyColor() {
        if (isHovered && !isRewinding) {
            sr.color = hoverColor;
        } else if (isRewinding) {
            sr.color = recallColor;
        } else {
            sr.color = defaultColor;
        }
    }
}
