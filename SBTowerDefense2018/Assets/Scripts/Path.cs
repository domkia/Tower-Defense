using System.Collections.Generic;

public class Path
{
    public List<HexTile> Waypoints { get; private set; }

    public HexTile Destination { get; private set; }

    /// <summary>
    /// Constructs an empty path with no waypoints.
    /// </summary>
    public Path()
    {
        Waypoints = new List<HexTile>();
    }

    /// <summary>
    /// Constructs a path from given waypoints
    /// </summary>
    /// <param name="waypoints">Collection of waypoints</param>
    public Path(IEnumerable<HexTile> waypoints)
    {
        Waypoints = new List<HexTile>(waypoints);
        Destination = Waypoints[Waypoints.Count - 1];
    }

    /// <summary>
    /// Appends a new waypoint to the path.
    /// </summary>
    /// <param name="waypoint">Waypoint to be added</param>
    public void AddWaypoint(HexTile waypoint)
    {
        Waypoints.Add(waypoint);
        Destination = Waypoints[Waypoints.Count - 1];
    }

}