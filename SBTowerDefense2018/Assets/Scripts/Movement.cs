using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 10f;

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        target = WayPoints.points[0];
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                GetNextWayPoint();
            }
        }
    }

    void GetNextWayPoint()
    {
        
        if (waypointIndex >= WayPoints.points.Length - 1)
        {
            LivesINFO.Lives--;
            
            Destroy(gameObject);

        }

        if (waypointIndex < WayPoints.points.Length - 1)
        {
            waypointIndex++;
            target = WayPoints.points[waypointIndex];
        }
    }
}
