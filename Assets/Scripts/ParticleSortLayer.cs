using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortLayer : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //Change Foreground to the layer you want it to display on 
        //You could prob. make a public variable for this
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Particles";
    }

    // Update is called once per frame
    void Update () {
        if (Time.timeScale < 0.01f)
        {
            GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
}
