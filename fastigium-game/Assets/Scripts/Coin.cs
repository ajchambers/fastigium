using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ISaveable {
    [SerializeField] private string id;
    GeneralManager gm;
    bool hasBeenCollected;

    public float hoverSpeed;
    public float hoverAmplitude;
    private float initialYpos;
    private float phaseOffset;

    public Color minColor = Color.red;
    public Color maxColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialYpos = transform.position.y;
        gm = FindObjectOfType<GeneralManager>();
        SetOffset();
        RandomizeColor();
    }

    private void SetOffset() {
        System.Random random = new System.Random();
        phaseOffset = random.Next(0, 100);
    }

    private void Update() {
        if (!TimeManager.tmInstance.isTimeStopped)
            Hover();
    }

    private void RandomizeColor() {
        float randomR = Random.Range(minColor.r, maxColor.r);
        float randomG = Random.Range(minColor.g, maxColor.g);
        float randomB = Random.Range(minColor.b, maxColor.b);
        float randomA = Random.Range(minColor.a, maxColor.a);

        Color randomColor = new Color(randomR, randomG, randomB, randomA);
        spriteRenderer.color = randomColor;
    }

    void Hover() {
        Vector2 p = transform.position;
        p.y = hoverAmplitude * Mathf.Cos(Time.time * hoverSpeed + phaseOffset) + initialYpos;
        transform.position = p;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            gm.AddPoints(10);
            hasBeenCollected = true;
            gameObject.SetActive(false);

            SaveManager.smInstance.SaveObject(this);
        }        
    }

    public void LoadData(GameData data) {
        data.coinData.TryGetValue(id, out bool hasBeenCollected);

        if (hasBeenCollected) {
            gameObject.SetActive(false);
        }

    }

    public void SaveData(ref GameData data) {        
        if (data.coinData.ContainsKey(id)) {
            data.coinData.Remove(id);
        }

        data.coinData.Add(id, hasBeenCollected);
    }
}
