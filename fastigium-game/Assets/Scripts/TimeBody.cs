using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TimeBody : MonoBehaviour
{
    private TimeManager timeManager;

    public float timeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private float timeBeforeAffectedTimer;
    
    private Rigidbody2D rb;
    private Vector3 lastPosition;
    private Transform t;
    
    private Vector2 recordedVelocity;
    private float recordedMagnitude;

    private bool canBeAffected;
    private bool isStopped;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        timeManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<TimeManager>();
        timeBeforeAffectedTimer = timeBeforeAffected;

        t = transform;
        lastPosition = t.position;
    }

    void Update() { 
        timeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if(timeBeforeAffectedTimer <= 0f) {
            canBeAffected = true; // Will be affected by timestop
        }

        if(canBeAffected && timeManager.isTimeStopped && !isStopped){
            StopTime();
        }
    }

    public void StopTime() {
        if(rb.velocity.magnitude >= 0f) { //If Object is moving
            recordedVelocity = rb.velocity.normalized; //records direction of movement
            recordedMagnitude = rb.velocity.magnitude; // records magitude of movement

            rb.velocity = Vector2.zero; //makes the rigidbody stop moving
            rb.isKinematic = true; //not affected by forces
            isStopped = true; // prevents this from looping
        }
    }

    public void ContinueTime() {
        rb.isKinematic = false;
        isStopped = false;
        // rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the recorded velocity when time continues
    }
}
