using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralManager : MonoBehaviour, ISaveable, IManager {
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;

    // flags
    public bool isPlayerAlive = true;

    public int score = 0;

    // saveable attributes
    public int deathCount;
    public static Vector3 respawnPoint;

    // setters and getters    
    public void SetRespawnPoint(Vector3 coords) {
        respawnPoint = coords;
    }
    public Vector3 GetRespawnPoint() {
        return respawnPoint;
    }

    public int GetDeathCount() {
        return deathCount;
    }

    private void Start() {
        LookForManagers();
    }

    public void LookForManagers() {
        gmInstance = this;
        scInstance = FindObjectOfType<SceneController>();
        smInstance = FindObjectOfType<SaveManager>();
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = FindObjectOfType<UIManager>();
    }

    public void KillPlayer() {
        isPlayerAlive = false;
        deathCount++;
        SubtractPoints(5);
        StartCoroutine(Respawn(0.5f));
    }

    public void AddPoints(int points) {
        score += points;
    }

    public void SubtractPoints(int points) {
        if (score >= 0)
            score -= points;

        if (score < 0)
            score = 0;
    }

    IEnumerator Respawn(float duration) {
        Rigidbody2D playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        transform.localScale = new Vector2(0, 0);
        yield return new WaitForSeconds(duration);
        // transform.localScale = newGeneralManager Vector2 (0, 0);
        playerRb.simulated = true;
        playerRb.transform.position = respawnPoint;
        isPlayerAlive = true;
    }

    public void LoadData(GameData data) {
        this.score = data.score;
        this.deathCount = data.deathCount;
        SetRespawnPoint(data.respawnPoint);  
    }

    public void SaveData(ref GameData data) {
        data.score = this.score;
        data.deathCount = this.deathCount;
        data.respawnPoint = this.GetRespawnPoint();
    }
}