using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EnemyData {
    public bool isDead;
    public Vector3 position;
    public float initialYpos;
    public bool facingRight;
    public bool hasFoundPlayer;
    public int skin;

    // enemy1 data builder
    public EnemyData(bool isDead, Vector3 position, bool facingRight, int skin) {
        this.isDead = isDead;
        this.position = position;
        this.initialYpos = 0;
        this.facingRight = facingRight;
        this.hasFoundPlayer = false;
        this.skin = skin;
    }
    
    // enemy2 data builder
    public EnemyData(bool isDead, Vector3 position, float initialYpos, bool facingRight, bool hasFoundPlayer, int skin) {
        this.isDead = isDead;
        this.position = position;
        this.initialYpos = initialYpos;
        this.facingRight = facingRight;
        this.hasFoundPlayer = hasFoundPlayer;
        this.skin = skin;
    }

    public override string ToString() {
        return "initialYpos: " + initialYpos;
        // return "isDead: " + isDead + ", position: " + position + ", facingRight: " 
        // + facingRight + ", hasFoundPlayer: " + hasFoundPlayer + ", skin: " + skin;
    }
}
