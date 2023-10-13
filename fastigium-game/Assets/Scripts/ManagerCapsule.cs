using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCapsule : MonoBehaviour {
    public static ManagerCapsule m;
    private void Awake() {
        BecomeASingleton();
    }
    
     public void BecomeASingleton() {
        if (m == null) {
            m = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }   
}
