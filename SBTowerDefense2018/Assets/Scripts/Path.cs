using System.Collections.Generic;

public class Path
{
    public List<Tile> Waypoints { get; private set; }

    public Tile Destination { get; private set; }

    /// <summary>
    /// Constructs an empty path with no waypoints.
    /// </summary>
    public Path()
    {
        Waypoints = new List<Tile>();
    }

    /// <summary>
    /// Constructs a path from given waypoints
    /// </summary>
    /// <param name="waypoints">Collection of waypoints</param>
    public Path(IEnumerable<Tile> waypoints)
    {
        Waypoints = new List<Tile>(waypoints);
        Destination = Waypoints[Waypoints.Count - 1];
    }

    /// <summary>
    /// Appends a new waypoint to the path.
    /// </summary>
    /// <param name="waypoint">Waypoint to be added</param>
    public void AddWaypoint(Tile waypoint)
    {
        Waypoints.Add(waypoint);
        Destination = Waypoints[Waypoints.Count - 1];
    }

}