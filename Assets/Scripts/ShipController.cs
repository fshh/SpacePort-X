using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
    public Queue<Vector3> waypoints;
    public float waypointRadius = 1.5f;
    public float damping = 0.1f;
    public float speed = 2.0f;
    public int scoreValue = 100;
    
    private Vector3 targetwaypoint;
    private Transform xform;
    private bool following;
    private LineRenderer liner;

    // Use this for initialization
    protected void Start() {
        waypoints = new Queue<Vector3>();
        liner = GetComponent<LineRenderer>();
        xform = GetComponent<Transform>();

        if (waypoints.Count <= 0) {
            following = false;
        } else {
            targetwaypoint = waypoints.Dequeue();
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
            xform.position = Vector3.MoveTowards(xform.position, targetwaypoint, speed * Time.deltaTime);

            // point ship towards waypoint
            xform.up = Vector3.Lerp(xform.up, targetwaypoint - xform.position, damping * Time.deltaTime);
            //xform.up = targetwaypoint - xform.position;

            // if waypoint has been reached, move to next waypoint (if it exists)
            if (Vector3.Distance(xform.position, targetwaypoint) <= waypointRadius) {
                if (waypoints.Count <= 0) {
                    following = false;
                } else {
                    targetwaypoint = waypoints.Dequeue();
                }
            }
        } else {
            // if ship has no waypoints, continue moving forwards indefinitely
            xform.position = Vector3.MoveTowards(xform.position, xform.position + xform.up, speed * Time.deltaTime);
        }
    }

    protected void LateUpdate() {
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
        waypoints = new Queue<Vector3>();

        // immediately set target waypoint on first click
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = xform.position.z;
        targetwaypoint = mousePos;
    }

    private void OnMouseDrag() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = xform.position.z;
        //Debug.Log("Mouse position: (" + mousePos.x + ", " + mousePos.y + ")");
        waypoints.Enqueue(mousePos);
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
        // TODO: play death animation
        Destroy(gameObject); 
    }
}
