using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ship")) {
            ShipController controller = collision.gameObject.GetComponent<ShipController>();
            controller.GetSucced(transform.position);
        }
        /*
        if (collision.gameObject.CompareTag("Hazard")) {
            AsteroidController controller = collision.gameObject.GetComponent<AsteroidController>();
            controller.Teleport(target.transform.position);
        }
        */
    }
}
