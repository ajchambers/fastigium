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

    private Vector3 lastPosition;
    private Transform t;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        timeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeManager>();
        TimeBeforeAffectedTimer = TimeBeforeAffected;

        t = transform;
        lastPosition = t.position;
    }

    void Update() { 
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if(TimeBeforeAffectedTimer <= 0f) {
            CanBeAffected = true; // Will be affected by timestop
        }

        if(CanBeAffected && timeManager.isTimeStopped && !IsStopped){
            StopTime();
        }
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
}
