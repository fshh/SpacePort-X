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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<ShipController>() && collision.gameObject.layer == this.gameObject.layer) {
            ShipController controller = collision.gameObject.GetComponent<ShipController>();
            controller.Score();
        }
    }
}
