using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public bool isTimeStopped;
    public int timeStopDuration;
    private Coroutine stopCo;
    private Coroutine cooldownCo;

    public int coolDownTime;
    public bool canStopTime;

    // UI
    public Image pauseIcon;
    public Slider slider;

    void Awake() {
        // set up UI
        pauseIcon = GameObject.Find("PauseIconImage").GetComponent<Image>();
        slider = GameObject.Find("CooldownTimer").GetComponent<Slider>();

        isTimeStopped = false;
        pauseIcon.enabled = false;
        canStopTime = true;
    }

    void Update() {
        // use right mouse toggle time freeze on and off
        if (Input.GetMouseButtonDown(1) && !isTimeStopped) {
            if (canStopTime) { // stop time if player is allowed to
                stopCo = StartCoroutine(StopTime());
            }
        } else {
            // if you toggle time off early, continue time
            if ((Input.GetMouseButtonDown(1) && isTimeStopped)) {
                ContinueTime();
            }
        }

        // if the player dies, continue time
        if (!GameManager.gmInstance.isPlayerAlive) {
            ContinueTime();
        }
    }

    public void ContinueTime() {
        isTimeStopped = false;
        pauseIcon.enabled = false;
        
        //Find every object with the Timebody or TimeBodyRewindable Component...
        var objects = FindObjectsOfType<TimeBody>();
        var rewindableObjects = FindObjectsOfType<TimeBodyRewindable>();
        
        //... and continue time in each of them.
        for (var i = 0; i < objects.Length; i++) {
            objects[i].GetComponent<TimeBody>().ContinueTime(); 
        }
        for (var i = 0; i < rewindableObjects.Length; i++) {
            rewindableObjects[i].GetComponent<TimeBodyRewindable>().ContinueTime();
        }

        // stop the timeStop coroutine
        if (stopCo != null) {
            StopCoroutine(stopCo);
        }
        
        // initiate cooldown
        cooldownCo = StartCoroutine(Cooldown());
    }

    IEnumerator StopTime() {
        isTimeStopped = true;
        pauseIcon.enabled = true;

        // decrease slider value to indicate time stop time running out
        slider.value = 1;

        for (int i = timeStopDuration - 1; i > -1; i--) {
            float progress = i / (float)timeStopDuration;
            yield return new WaitForSeconds(1);
            slider.value = progress;
        }

        ContinueTime();
    }

    IEnumerator Cooldown() {
        canStopTime = false;
        slider.value = 0;

        for (int i = 1; i < coolDownTime + 1; i++) {
            yield return new WaitForSeconds(1);
            float progress = i / (float)coolDownTime;
            slider.value = progress;
        }

        canStopTime = true;
    }

    
}