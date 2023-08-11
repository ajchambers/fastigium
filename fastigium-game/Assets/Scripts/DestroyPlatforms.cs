using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatforms : MonoBehaviour
{
    public Spanwer spawner;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("FallingPlatform")) {
            Destroy(collision.gameObject);
            spawner.objectDestroyed();
        }
    }
}
