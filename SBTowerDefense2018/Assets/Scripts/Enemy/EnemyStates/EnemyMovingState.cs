using UnityEngine;

public class EnemyMovingState : IEnemyState
{
    private Path path;
    private int targetWaypoint;
    private HexTile currentlyOn;
    private float moveProgress;

    //public static System.Action<Enemy, HexTile> onEnteredTile;

    public EnemyMovingState(Enemy enemy, Path path)
    {
        this.path = path;
        this.targetWaypoint = 1;
        this.currentlyOn = path[0];
        this.moveProgress = 0;
        //Debug.Log("Currently on: " + currentlyOn);
    }

    public void UpdateState(Enemy enemy)
    {
        //Update position
        moveProgress += enemy.Speed * Time.deltaTime;
        enemy.transform.position = Vector3.Lerp(path[targetWaypoint - 1].worldPos, path[targetWaypoint].worldPos, moveProgress);

        if (moveProgress >= 0.5f && path[targetWaypoint] != currentlyOn)
        {
            currentlyOn = path[targetWaypoint];
            //TODO: Let towers/tiles know this enemy has just entered

            if (currentlyOn == path.Destination)
                enemy.Attack(null);         //TODO: Pass actual tower instead of null
        }

        if (moveProgress >= 1f)
        {
            moveProgress = 0f;
            targetWaypoint++;
            if (targetWaypoint > path.Waypoints.Count - 1)
                enemy.Idle();               //If destination is not tower, just chill in that spot
        }
    }
}
