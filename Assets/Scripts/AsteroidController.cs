using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public float speed = 1.5f;
    public GameObject explosion;

    private Transform xform;
    private bool teleport;

	// Use this for initialization
	void Start () {
        xform = GetComponent<Transform>();
        teleport = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        xform.position = Vector3.MoveTowards(xform.position, xform.position + xform.up, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision Detected at " + collision.GetContact(0).point);
        if (collision.gameObject.CompareTag("Ship") || collision.gameObject.CompareTag("Hazard")) {
            Die();
        }
    }

    public void Die() {
        Instantiate(explosion, xform.position, xform.rotation);
        Destroy(gameObject);
    }

    public void Teleport(Vector3 dest) {
        if (teleport) {
            xform.position = dest;

            teleport = false;
            Invoke("AllowTeleport", 2f);
        }
    }

    public void AllowTeleport() {
        teleport = true;
    }
}
