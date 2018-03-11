using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores waypoints of a path in a list.
/// </summary>
public class Path
{
    /// <summary>
    /// A list of waypoints in this path.
    /// </summary>
    public List<Vector3> Waypoints { get; private set; }

    /// <summary>
    /// Constructs an empty path with no waypoints.
    /// </summary>
    public Path()
    {
        Waypoints = new List<Vector3>();
    }

    /// <summary>
    /// Given a collection of waypoint locations, constructs a path from them in order.
    /// </summary>
    /// <param name="waypointLocations">Collection of waypoint locations</param>
    public Path(IEnumerable<Vector3> waypointLocations)
    {
        Waypoints = new List<Vector3>(waypointLocations);
    }

    /// <summary>
    /// Appends a waypoint to the path.
    /// </summary>
    /// <param name="location">Location of waypoint.</param>
    public void AddWaypoint(Vector3 location)
    {
        Waypoints.Add(location);
    }

    /// <summary>
    /// Instantiates path in the scene.
    /// </summary>
    /// <param name="parent">Parent transform</param>
    public void InstantiatePath(Transform parent)
    {
        if(Waypoints.Count > 0)
        {
            GameObject[] waypointObjects = new GameObject[Waypoints.Count];
            for(int i = 0; i < Waypoints.Count; i++)
            {
                waypointObjects[i] = new GameObject(string.Format("Waypoint {0}", i + 1));
                waypointObjects[i].transform.position = Waypoints[i];
                waypointObjects[i].transform.parent = parent;
            }
        }
    }
}