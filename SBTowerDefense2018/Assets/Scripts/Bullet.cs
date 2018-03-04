using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // Enemy position that the bullet is targetting (heading towards)
    private Transform target;
    // Speed of the bullet
    public float Speed = 0.5f;
    // Tag of enemy (passed in by tower)
    private string enemyTag;

    /// <summary>
    /// Sets the bullet's target.
    /// </summary>
    /// <param name="_target">Enemy target</param>
    /// <param name="_enemyTag">Tag of enemy</param>
    public void Seek(Transform _target, string _enemyTag)
    {
        target = _target;
        enemyTag = _enemyTag;
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

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    /// <summary>
    /// This method gets called when the bullet collides with something.
    /// </summary>
    /// <param name="other">Other object's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(enemyTag))
        {
            // The bullet hit an enemy
            Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }
}
