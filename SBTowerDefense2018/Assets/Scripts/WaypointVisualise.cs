using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointVisualise : MonoBehaviour {

    public Transform[] waypoints;

    private void OnDrawGizmos()
    {
        if (waypoints.Length <= 0)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(waypoints[waypoints.Length-1].position, Vector3.one);
        for (int i = 0; i < waypoints.Length-1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            Gizmos.DrawSphere(waypoints[i].position, 0.1f);
        }
    }
}
