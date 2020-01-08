using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
    public Queue<Vector3> waypoints;
    public float waypointRadius = 1.5f;
    public float damping = 0.1f;
    public float speed = 2.0f;
    public int scoreValue = 100;
    public float cooldownDuration = 3f;
    public Color cooldownColor;
    public Color readyColor;
    public GameObject explosion;
    public GameObject warning;
    
    private Vector3 targetWaypoint, lastDrawnWaypoint;
    private Transform xform;
    private bool following, onCooldown, teleport;
    private LineRenderer liner;
    private float cooldownRemaining;

    // Use this for initialization
    protected void Start() {
        waypoints = new Queue<Vector3>();
        liner = GetComponent<LineRenderer>();
        liner.startColor = readyColor;
        liner.endColor = readyColor;
        xform = GetComponent<Transform>();
        onCooldown = false;
        teleport = true;

        if (waypoints.Count <= 0) {
            following = false;
        } else {
            targetWaypoint = waypoints.Dequeue();
            following = true;
        }
    }

    // calculates a new heading and moves along it
    protected void FixedUpdate() {
        if (waypoints.Count > 0) {
            following = true;
        }

        if (following) {
            // move ship towards waypoint
            xform.position = Vector3.MoveTowards(xform.position, targetWaypoint, speed * Time.deltaTime);

            // point ship towards waypoint
            xform.up = Vector3.Lerp(xform.up, targetWaypoint - xform.position, damping * Time.deltaTime);
            //xform.up = targetwaypoint - xform.position;

            // if waypoint has been reached, move to next waypoint (if it exists)
            if (Vector3.Distance(xform.position, targetWaypoint) <= waypointRadius) {
                if (waypoints.Count <= 0) {
                    following = false;
                } else {
                    targetWaypoint = waypoints.Dequeue();
                }
            }
        } else {
            // if ship has no waypoints, continue moving forwards indefinitely
            xform.position = Vector3.MoveTowards(xform.position, xform.position + xform.up, speed * Time.deltaTime);
        }
    }

    protected void LateUpdate() {
        // if cooldown has not yet reset, display color accordingly
        if (onCooldown) {
            cooldownRemaining = Mathf.Max(0, cooldownRemaining - Time.deltaTime);
            Color currColor = Color.Lerp(readyColor, cooldownColor, cooldownRemaining / cooldownDuration);
            liner.startColor = currColor;
            liner.endColor = currColor;
        }

        // if there are no waypoints, draw nothing; otherwise, draw path of ship
        if (waypoints.Count <= 0) {
            liner.positionCount = 0;
            return;
        } else {
            Vector3[] vertices = new Vector3[waypoints.Count + 1];
            vertices[0] = xform.position;
            int count = 1;
            foreach (Vector3 v in waypoints) {
                vertices[count] = v;
                count++;
            }

            liner.positionCount = waypoints.Count + 1;
            liner.SetPositions(vertices);
        }
    }

    private void OnMouseDown() {
        if (onCooldown && cooldownRemaining == 0f) {
            onCooldown = false;
        }
        if (!onCooldown) {
            liner.startColor = cooldownColor;
            liner.endColor = cooldownColor;
            waypoints = new Queue<Vector3>();

            // immediately set target waypoint on first click
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = xform.position.z;
            targetWaypoint = mousePos;
        }
    }

    private void OnMouseDrag() {
        if (!onCooldown) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = xform.position.z;
            if (Vector3.Distance(lastDrawnWaypoint, mousePos) > waypointRadius) {
                waypoints.Enqueue(mousePos);
                lastDrawnWaypoint = mousePos;
            }
            //Debug.Log("Mouse position: (" + mousePos.x + ", " + mousePos.y + ")");
        }
    }

    private void OnMouseUp() {
        if (!onCooldown) {
            onCooldown = true;
            cooldownRemaining = cooldownDuration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision Detected at " + collision.GetContact(0).point);
        if (collision.gameObject.CompareTag("Ship") || collision.gameObject.CompareTag("Hazard")) {
            Die();
            GameManager.instance.GameOver();
        }
    }

    public void Score() {
        ScoreManager.score += scoreValue;
        // TODO: play fadeout animation, score popup?
        Destroy(gameObject);
    }

    public void Die() {
        Instantiate(explosion, xform.position, xform.rotation);
        Destroy(gameObject); 
    }

    public void Teleport(Vector3 dest) {
        if (teleport) {
            xform.up = targetWaypoint - xform.position;
            xform.position = dest;

            waypoints = new Queue<Vector3>();
            following = false;
            cooldownRemaining = 0f;
            onCooldown = true;
            
            teleport = false;
            Invoke("AllowTeleport", 2f);
        }
    }

    public void AllowTeleport() {
        teleport = true;
    }

    IEnumerator Succ(Vector3 center) {
        Vector3 origScale = xform.localScale;
        Vector3 origPos = xform.position;
        for (float t = 1f; t > 0f; t -= 0.01f) {
            xform.localScale = origScale * t;
            xform.position = Vector3.Lerp(center, origPos, t);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Destroy(gameObject);
    }

    public void GetSucced(Vector3 center) {
        waypoints = new Queue<Vector3>();
        StartCoroutine(Succ(center));
        GameManager.instance.GameOver();
    }

    public GameObject GetWarning() {
        return warning;
    }
}
