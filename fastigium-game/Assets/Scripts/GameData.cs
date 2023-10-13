using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public int deathCount;
    public Vector3 playerPosition;

    public string currentScene;
    public Vector3 respawnPoint;
    public SerializableDictionary <string, EnemyData> enemy1data;
    public SerializableDictionary <string, EnemyData> enemy2data;

    // default game data when game starts
    public GameData() {
        // player controller info
        this.deathCount = 0;
        this.playerPosition = new Vector3(13, -9, 0);

        // game manager info
        this.respawnPoint = new Vector3(13, -9, 0);

        // scene controller
        this.currentScene = "t0";

        // enemy1s
        this.enemy1data = new SerializableDictionary<string, EnemyData>();

        // enemy2s
        this.enemy2data = new SerializableDictionary<string, EnemyData>();
    }
}