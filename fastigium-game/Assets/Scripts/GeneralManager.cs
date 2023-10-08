using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralManager : MonoBehaviour, ISaveable {
    public static GeneralManager gmInstance;

    // flags
    public bool isPlayerAlive = true;

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

    // keep the same game manager object throughout all levels
    private void Awake() {
        if (gmInstance == null) {
            gmInstance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void KillPlayer() {
        isPlayerAlive = false;
        deathCount++;
        StartCoroutine(Respawn(1.0f));
    }

    IEnumerator Respawn(float duration) {
        Rigidbody2D playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();

        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        transform.localScale = new Vector2(0, 0);
        yield return new WaitForSeconds(duration);
        playerRb.transform.position = respawnPoint;
        // transform.localScale = newGeneralManager Vector2 (0, 0);
        playerRb.simulated = true;
        isPlayerAlive = true;
    }

    public void LoadData(GameData data) {
        this.deathCount = data.deathCount;
        SetRespawnPoint(data.respawnPoint);  
    }

    public void SaveData(ref GameData data) {
        data.deathCount = this.deathCount;
        data.respawnPoint = this.GetRespawnPoint();
    }
}