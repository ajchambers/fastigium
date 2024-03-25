using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class VisualsManager : MonoBehaviour {
    public Color spriteColor;
    public Color bgColor;

    public Camera cam;
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start() {
        
    }

    void Update() {
        UpdateSprites();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            spriteColor = Color.white;
            bgColor = Color.white;            
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        int sceneNum = System.Convert.ToInt32(currentSceneName);

        if (sceneNum >= 1 && sceneNum <= 28 && sceneNum % 1 == 0) {
            SetColors(sceneNum);
        } else {
            spriteColor = Color.white;
            bgColor = Color.white;
        }
        
        UpdateRegTilemap();
        UpdateFineTilemap("Dangerous");
        UpdateFineTilemap("Climbable");
        UpdateCamera();
        UpdateBackgroundColor();
    }

    private void SetColors(int value) {
        // set sprite color to calcuated color
        spriteColor = CalculateColor(value);

        // set bg color to augmented sprite color
        bgColor = spriteColor + new Color(0.25f, 0.25f, 0.25f, 1);

        // color.grey = (0.5, 0.5, 0.5, 1).
    }

    private Color CalculateColor(int sceneNum){
        Color[] colors = {
            new Color(252f / 255f, 210f / 255f, 59f / 255f),
            new Color(253f / 255f, 205f / 255f, 55f / 255f),
            new Color(253f / 255f, 200f / 255f, 51f / 255f),
            new Color(254f / 255f, 196f / 255f, 47f / 255f),
            new Color(255f / 255f, 191f / 255f, 43f / 255f),
            new Color(255f / 255f, 186f / 255f, 39f / 255f),
            new Color(255f / 255f, 181f / 255f, 36f / 255f),
            new Color(255f / 255f, 176f / 255f, 33f / 255f),
            new Color(255f / 255f, 171f / 255f, 31f / 255f),
            new Color(255f / 255f, 166f / 255f, 29f / 255f),
            new Color(255f / 255f, 161f / 255f, 27f / 255f),
            new Color(255f / 255f, 155f / 255f, 26f / 255f),
            new Color(255f / 255f, 150f / 255f, 25f / 255f),
            new Color(255f / 255f, 144f / 255f, 25f / 255f),
            new Color(255f / 255f, 139f / 255f, 26f / 255f),
            new Color(255f / 255f, 133f / 255f, 26f / 255f),
            new Color(255f / 255f, 128f / 255f, 27f / 255f),
            new Color(255f / 255f, 122f / 255f, 28f / 255f),
            new Color(255f / 255f, 116f / 255f, 30f / 255f),
            new Color(255f / 255f, 110f / 255f, 31f / 255f),
            new Color(255f / 255f, 103f / 255f, 33f / 255f),
            new Color(255f / 255f, 97f / 255f, 35f / 255f),
            new Color(255f / 255f, 90f / 255f, 37f / 255f),
            new Color(255f / 255f, 83f / 255f, 39f / 255f),
            new Color(255f / 255f, 75f / 255f, 41f / 255f),
            new Color(255f / 255f, 67f / 255f, 43f / 255f),
            new Color(254f / 255f, 57f / 255f, 45f / 255f),
            new Color(253f / 255f, 47f / 255f, 47f / 255f),
            // new Color(blue)
        };

        // Calculate the index in the colors array
        int index = (sceneNum - 1) * (colors.Length - 1) / 27;

        // Return the corresponding color
        return colors[index];
    }

    private void UpdateRegTilemap() {
        Tilemap tilemap = GameObject.Find("RegPlatforms").GetComponent<Tilemap>();

        for (int x = -18; x <= 17; x++) {
            for (int y = 9; y >= -10; y--) {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTileFlags(tilePosition, TileFlags.None);
                tilemap.SetColor(tilePosition, spriteColor);
            }
        }       
    }

    private void UpdateFineTilemap(string mapName) {
        Tilemap tilemap = GameObject.Find(mapName).GetComponent<Tilemap>();

        for (int x = -36; x <= 35; x++) {
            for (int y = 19; y >= -20; y--) {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTileFlags(tilePosition, TileFlags.None);
                tilemap.SetColor(tilePosition, spriteColor);
            }
        } 
    }

    private void UpdateSprites() {
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer sr in allSprites) {
            sr.color = spriteColor;
        }
    }

    private void UpdateBackgroundColor() {
        if (cam != null) {
            cam.backgroundColor = bgColor;
        } else {
            UpdateCamera();
            cam.backgroundColor = bgColor;
        }   
    }

    private void UpdateCamera() {
        cam = FindObjectOfType<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}
