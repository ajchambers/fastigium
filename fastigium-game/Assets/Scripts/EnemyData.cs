using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EnemyData {
    public bool isDead;
    public Vector3 position;
    public int skin;

    public EnemyData(bool isDead, Vector3 position, int skin) {
        this.isDead = isDead;
        this.position = position;
        this.skin = skin;
    }

    public EnemyData() {
        this.isDead = false;
        this.position = new Vector3(0, 0);
        this.skin = 1;
    }

    public override string ToString() {
        return "isDead: " + isDead + ", position: " + position + ", skin: " + skin;
    }
}
