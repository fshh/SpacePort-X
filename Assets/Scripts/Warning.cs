using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour {

    public int duration = 3;

    private float timer;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        timer = 0f;
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Flash());
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= (float)duration + 0.5f) {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
	}

    IEnumerator Flash() {
        for (int i=0; i < duration; i++) {
            float f = 0f;
            while (f < 1f) {
                sprite.color = Color.Lerp(Color.white, Color.clear, f);
                f += Time.deltaTime * 2f;
                yield return null;
            }
            f = 1f;
            while (f > 0f) {
                sprite.color = Color.Lerp(Color.white, Color.clear, f);
                f -= Time.deltaTime * 2f;
                yield return null;
            }
        }

        float t = 0f;
        while (t < 1f) {
            sprite.color = Color.Lerp(Color.white, Color.clear, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }
    }
}
