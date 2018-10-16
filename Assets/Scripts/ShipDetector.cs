using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<ShipController>() && other.gameObject.layer == this.gameObject.layer) {
            ShipController controller = other.gameObject.GetComponent<ShipController>();
            controller.Score();
        }
    }
}
