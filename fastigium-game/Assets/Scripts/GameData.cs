using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public int deathCount;
    public Vector3 playerPosition;

    public string currentScene;
    public Vector3 respawnPoint;
    public SerializableDictionary <string, bool> enemy1s;

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
        this.enemy1s = new SerializableDictionary<string, bool>();

        // enemy2s
            // position
            // whether it's dead

        // platforms

        // missiles

        // dialogue
    }
}