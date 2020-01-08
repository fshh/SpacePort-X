using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour {

    public GameObject target;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ship")) {
            ShipController controller = collision.gameObject.GetComponent<ShipController>();
            controller.Teleport(target.transform.position);
        }

        if (collision.gameObject.CompareTag("Hazard")) {
            AsteroidController controller = collision.gameObject.GetComponent<AsteroidController>();
            controller.Teleport(target.transform.position);
        }
    }
}
