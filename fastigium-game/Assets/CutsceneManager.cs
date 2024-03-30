using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour {
    public void GoToStats() {
        SceneManager.LoadScene("ScoreScreen");
    }
}
