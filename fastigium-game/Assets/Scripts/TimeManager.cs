using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public bool isTimeStopped;
    // public Image pauseIcon;

    void Awake() {
        isTimeStopped = false;
        // pauseIcon.enabled = false;
    }

    void Update() {
        // use right mouse toggle time freeze on and off
        if (Input.GetMouseButtonDown(1) && !isTimeStopped) {
            StopTime();
        } else {   
            if (Input.GetMouseButtonDown(1) && isTimeStopped) {
                ContinueTime();
            }
        }
    }

    public void ContinueTime() {
        isTimeStopped = false;
        // pauseIcon.enabled = false;
        
        var objects = FindObjectsOfType<TimeBody>();  //Find Every object with the Timebody Component
        
        for (var i = 0; i < objects.Length; i++) {
            objects[i].GetComponent<TimeBody>().ContinueTime(); //continue time in each of them
        }
    }

    public void StopTime() {
        isTimeStopped = true;
        // pauseIcon.enabled = true;
    }

    
}