using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour {
    public Camera cam;
    public GameObject redShip;
    public GameObject greenShip;
    public GameObject blueShip;
    public float initialDelay = 2f;
    public float spawnDelay = 10f;
    public float rateIncreaseDelay = 20f;
    public float rateIncreaseFactor = 0.1f;
    public float spawnOffsetDistance = 2f;
    public float minimumSpawnDelay = 2f;

    private float timeSinceSpawn = 0f;
    private int lastSpawnRegion = 0;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", initialDelay, spawnDelay);

        InvokeRepeating("IncreaseSpawnSpeed", initialDelay + rateIncreaseDelay, rateIncreaseDelay);
	}

    // Update is called once per frame
    void Update() {
        timeSinceSpawn += Time.deltaTime;
    }
    
    // increases the spawn rate as time goes on
    void IncreaseSpawnSpeed() {
        Debug.Log("Increasing spawnrate");
        CancelInvoke("Spawn");

        spawnDelay -= (spawnDelay * rateIncreaseFactor);

        if (spawnDelay < minimumSpawnDelay) {
            spawnDelay = minimumSpawnDelay;
        }

        if (spawnDelay - timeSinceSpawn < 0f) {
            InvokeRepeating("Spawn", 0, spawnDelay);
        } else {
            InvokeRepeating("Spawn", spawnDelay - timeSinceSpawn, spawnDelay);
        }
    }

    // spawns a ship just off the edge of the screen
    void Spawn() {
        GameObject ship;
        int shipType = (int)Mathf.Floor(Random.Range(0.0f, 2.99f));
        switch (shipType) {
            case 0:
                ship = redShip;
                break;
            case 1:
                ship = greenShip;
                break;
            case 2:
                ship = blueShip;
                break;
            default:
                ship = redShip;
                Debug.Log("Random number generator did not choose one of the three ship types");
                break;
        }

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        
        int spawnRegion;
        do {
            spawnRegion = (int)Mathf.Floor(Random.Range(0.0f, 3.99f));
        } while (spawnRegion == lastSpawnRegion);
        lastSpawnRegion = spawnRegion;

        Vector3 spawnpoint;
        Vector3 lookpoint;
        switch (spawnRegion) {
            case 0: // top
                spawnpoint = new Vector3(Random.Range(-horzExtent, horzExtent), vertExtent + spawnOffsetDistance, 0);
                lookpoint = new Vector3(Random.Range(-horzExtent, horzExtent), -vertExtent, 0);
                break;
            case 1: // bottom
                spawnpoint = new Vector3(Random.Range(-horzExtent, horzExtent), -vertExtent - spawnOffsetDistance, 0);
                lookpoint = new Vector3(Random.Range(-horzExtent, horzExtent), vertExtent, 0);
                break;
            case 2: // left
                spawnpoint = new Vector3(-horzExtent - spawnOffsetDistance, Random.Range(-vertExtent, vertExtent), 0);
                lookpoint = new Vector3(horzExtent, Random.Range(-vertExtent, vertExtent), 0);
                break;
            case 3: // right
                spawnpoint = new Vector3(horzExtent + spawnOffsetDistance, Random.Range(-vertExtent, vertExtent), 0);
                lookpoint = new Vector3(-horzExtent, Random.Range(-vertExtent, vertExtent), 0);
                break;
            default:
                spawnpoint = new Vector3(horzExtent + spawnOffsetDistance, vertExtent + spawnOffsetDistance);
                lookpoint = new Vector3(-horzExtent - spawnOffsetDistance, -vertExtent - spawnOffsetDistance);
                Debug.Log("Random number generator did not choose one of the four spawn regions");
                break;
        }

        GameObject shipInstance = Instantiate(ship, spawnpoint, Quaternion.FromToRotation(lookpoint, spawnpoint));
        shipInstance.transform.up = lookpoint - shipInstance.transform.position;

        timeSinceSpawn = 0f;
    }
}
