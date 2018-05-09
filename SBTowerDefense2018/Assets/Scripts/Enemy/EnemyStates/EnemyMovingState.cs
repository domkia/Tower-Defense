using UnityEngine;

public class EnemyMovingState : IEnemyState
{
    private Enemy parent;
    private Path path;                      //Path to walk along
    private int targetWaypoint;
    private float moveProgress;             //0.0 -- 1.0

    public EnemyMovingState(Path path, Enemy parent)
    {
        this.parent = parent;
        this.parent.currentlyOn.EnemyEnter(parent);            //Enemy have just spawned on this tile
        this.path = path;
        this.targetWaypoint = 1;
        this.moveProgress = 0;
        HexTile.OnTileTypeChanged += RecalculatePath;
    }

    public void UpdateState()
    {
       
        //Update position
        moveProgress += parent.Speed * Time.deltaTime;
        parent.transform.position = Vector3.Lerp(path[targetWaypoint - 1].worldPos, path[targetWaypoint].worldPos, moveProgress);

        Vector3 nextPos = path[targetWaypoint].worldPos;
        parent.transform.LookAt(nextPos);
        //Calculate when this enemy enters / leaves current tile
        if (moveProgress >= 0.5f && path[targetWaypoint] != this.parent.currentlyOn)
        {
            HexTile next = path[targetWaypoint];
            this.parent.currentlyOn.EnemyExit(parent);          //Exit last visited tile
            this.parent.currentlyOn = next;
            this.parent.currentlyOn.EnemyEnter(parent);         //Enter new tile

            
            //Finally attack the base / other tower
            if (this.parent.currentlyOn == path.Destination)
            {
                HexTile.OnTileTypeChanged -= RecalculatePath;
                parent.Attack();
                return;
            }
        }

        //Change to the next waypoint / destination
        if (moveProgress >= 1f)
        {
            moveProgress = 0f;
            targetWaypoint++;

            nextPos = path[targetWaypoint].worldPos;
            parent.transform.LookAt(nextPos);
            //TODO: this may be useful later
            //This doesn't get called, unless enemy is not supposed to be attacking at all.
            if (targetWaypoint > path.Waypoints.Count - 1)
                parent.Idle();               //For now, just chill in that spot
        }
    }

    private void RecalculatePath(HexTile t)
    {
        //Save old tiles (need for interpolating)
        HexTile oldPrev = path[targetWaypoint - 1];
        HexTile old = path[targetWaypoint];

        //Get new path
        path = Pathfinding.GetPath(parent.currentlyOn, HexGrid.Instance.CenterTile);
        if (path == null)
        {
            parent.Idle();                                  //If no path is found, chill
            return;
        }

        //Append first few tiles
        if (moveProgress < 0.5f)
        {
            if (path[1] != old)
            {
                path.AddFirst(old);
                moveProgress = 1.0f - moveProgress;
            }
        }
        else
        {
            if (path[1] != oldPrev)
                path.AddFirst(oldPrev);
            else
                moveProgress = 1.0f - moveProgress;
        }
        targetWaypoint = 1;
    }
}
