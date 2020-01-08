using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracker : MonoBehaviour {
    private Text text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        string minutes = Mathf.Floor(Time.timeSinceLevelLoad / 60).ToString("00");
        string seconds = Mathf.Floor(Time.timeSinceLevelLoad % 60).ToString("00");
        text.text = string.Format("{0}:{1}", minutes, seconds);
	}
}
