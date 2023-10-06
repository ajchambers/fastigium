using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData  {
    //key
    string sceneName;

    // values
    bool hasBeenEnteredBefore;
    GameObject[] enemy1s;
    GameObject[] enemy2s;
    GameObject[] missiles;
    GameObject[] platformSpawners;
    GameObject[] platforms;
    
    GameObject lastCheckpoint;

    GameObject player;

    public SceneData(  string sceneName,

                bool hasBeenEnteredBefore,
                GameObject[] enemy1s,
                GameObject[] enemy2s,
                GameObject[] missiles,
                GameObject[] platformSpawners,
                GameObject[] platforms,

                GameObject lastCheckpoint,

                GameObject player) {
            this.sceneName = sceneName;

            this.hasBeenEnteredBefore = hasBeenEnteredBefore;
            this.enemy1s = enemy1s;
            this.enemy2s = enemy2s;
            this.missiles = missiles;
            this.platformSpawners = platformSpawners;
            this.platforms = platforms;

            this.lastCheckpoint = lastCheckpoint;
            this.player = player;
        }
}

