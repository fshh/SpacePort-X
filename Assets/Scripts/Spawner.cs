using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Camera cam;
    public GameObject[] ships;
    public GameObject warning;
    public GameObject asteroid;
    public GameObject blackhole;
    public float initialDelay = 2f;
    public float spawnDelay = 10f;
    public float rateIncreaseDelay = 20f;
    public float rateIncreaseFactor = 0.1f;
    public float spawnOffsetDistance = 2f;
    public float minimumSpawnDelay = 2f;

    private float timeSinceSpawn = 0f;
    private int lastSpawnRegion = 0;
    private bool holeCanSpawn = true;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnShip", initialDelay, spawnDelay);

        InvokeRepeating("IncreaseSpawnSpeed", initialDelay + rateIncreaseDelay, rateIncreaseDelay);

        InvokeRepeating("SpawnAsteroid", initialDelay, spawnDelay / 3);

        InvokeRepeating("SpawnBlackHole", initialDelay, spawnDelay / 3);
    }

    // Update is called once per frame
    void Update() {
        timeSinceSpawn += Time.deltaTime;
    }
    
    // increases the spawn rate as time goes on
    void IncreaseSpawnSpeed() {
        CancelInvoke("SpawnShip");

        spawnDelay -= (spawnDelay * rateIncreaseFactor);

        if (spawnDelay < minimumSpawnDelay) {
            spawnDelay = minimumSpawnDelay;
        }

        if (spawnDelay - timeSinceSpawn < 0f) {
            InvokeRepeating("SpawnShip", 0, spawnDelay);
        } else {
            InvokeRepeating("SpawnShip", spawnDelay - timeSinceSpawn, spawnDelay);
        }
        Debug.Log("Increasing spawnrate to " + spawnDelay);
    }

    GameObject Spawn(GameObject obj) {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        int spawnRegion;
        do {
            spawnRegion = Random.Range(0, 4);
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

        GameObject instance = Instantiate(obj, spawnpoint, Quaternion.FromToRotation(lookpoint, spawnpoint));
        instance.transform.up = lookpoint - instance.transform.position;

        Vector3 warningPoint = Vector3.MoveTowards(spawnpoint, lookpoint, spawnOffsetDistance + 0.5f);

        if (instance.CompareTag("Ship")) {
            GameObject shipWarning = instance.GetComponent<ShipController>().GetWarning();
            Instantiate(shipWarning, warningPoint, Quaternion.identity);
        } else {
            Instantiate(warning, warningPoint, Quaternion.identity);
        }

        timeSinceSpawn = 0f;

        return instance;
    }

    // spawns a ship just off the edge of the screen
    void SpawnShip() {
        Spawn(ships[Random.Range(0, ships.Length)]);
    }

    void SpawnAsteroid() {
        if (Random.Range(0, 4) == 0) {
            Spawn(asteroid);
        }
    }

    void SpawnBlackHole() {
        if (Random.Range(0, 10) == 0 && holeCanSpawn) {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            Vector3 spawnpoint = new Vector3((Random.value - 0.5f) * horzExtent, (Random.value - 0.5f) * vertExtent, 0);
            Instantiate(warning, spawnpoint, Quaternion.identity);
            StartCoroutine(BlackHoleRoutine(spawnpoint));
        }
    }

    IEnumerator BlackHoleRoutine(Vector3 spawnpoint ) {
        holeCanSpawn = false;
        yield return new WaitForSeconds(2.5f);
        GameObject hole = Instantiate(blackhole, spawnpoint, Quaternion.identity);
        yield return new WaitForSeconds(7f);
        Destroy(hole);
        yield return new WaitForSeconds(10f);
        holeCanSpawn = true;
    }
}
