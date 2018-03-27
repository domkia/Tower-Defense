using System;
using System.Collections.Generic;

public class Pathfinding
{
    private HexGrid grid;

    public Pathfinding(HexGrid grid)
    {
        this.grid = grid;
    }

    public List<HexTile> GetPath(HexTile fromTile, HexTile toTile)
    {
        foreach (HexTile tile in grid)
            if(tile != null)
                tile.Reset();

        if (fromTile == null || toTile == null)
            throw new Exception("fromTile or toTile null in Pathfinding class");

        List<HexTile> open = new List<HexTile>();
        HashSet<HexTile> closed = new HashSet<HexTile>();
        open.Add(fromTile);

        while (open.Count > 0)
        {
            //Find tile with smallest fCost
            //TODO: write some sort of priority queue
            HexTile current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].FCost < current.FCost || open[i].FCost == current.FCost)
                    if(open[i].hCost < current.hCost)
                        current = open[i];
            }

            open.Remove(current);
            closed.Add(current);

            if (current == toTile)
            {
                List<HexTile> path = new List<HexTile>();
                HexTile tile = toTile;
                while (tile != fromTile)
                {
                    path.Add(tile.prev);
                    tile = tile.prev;
                }
                path.Reverse();
                path.Add(toTile);
                return path;
            }

            foreach (HexTile neighbour in grid.GetNeighbours(current))
            {
                if (neighbour.type != TileType.Empty || closed.Contains(neighbour))
                    continue;

                int cost = current.gCost + grid.GetDistance(current, neighbour);
                //Debug.Log("cost from: " + current + " to " + neighbour + " is " + cost);
                if (cost < neighbour.gCost || !open.Contains(neighbour))
                {
                    neighbour.gCost = cost;
                    neighbour.hCost = grid.GetDistance(neighbour, toTile);
                    neighbour.prev = current;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }
            }
        }

        return null;
    }
}
