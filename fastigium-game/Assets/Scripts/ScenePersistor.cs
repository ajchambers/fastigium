using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistor : MonoBehaviour {
    public static ScenePersistor s;
    private void Awake() {
        BecomeASingleton();
    }
    
     public void BecomeASingleton() {
        if (s == null) {
            s = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
