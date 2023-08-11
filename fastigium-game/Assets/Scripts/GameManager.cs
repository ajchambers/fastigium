using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;
    public static Vector3 respawnPoint;

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
        StartCoroutine(Respawn(1.0f));
    }

    IEnumerator Respawn(float duration) {
        Rigidbody2D playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();

        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        transform.localScale = new Vector2(0, 0);
        yield return new WaitForSeconds(duration);
        playerRb.transform.position = respawnPoint;
        // transform.localScale = new Vector2 (0, 0);
        playerRb.simulated = true;
    }

    public void setRespawnPoint(Vector3 coords) {
        respawnPoint = coords;
    }
}
