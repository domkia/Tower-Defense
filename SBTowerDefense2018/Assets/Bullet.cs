using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // Enemy position that the bullet is targetting (heading towards)
    private Transform target;
    // Speed of the bullet
    public float Speed = 0.5f;

    /// <summary>
    /// Sets the bullet's target.
    /// </summary>
    /// <param name="_target">Enemy target</param>
    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        // If the bullet has no target, destroy it.
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = Speed * Time.deltaTime;

        // If the bullet is closer to the target than the distance it travelled, it hit the target.
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    /// <summary>
    /// Right now it just prints out 'Hit!' to the console when the bullet hits the target.
    /// </summary>
    private void HitTarget()
    {
        Debug.Log("Hit!");
    }
}
