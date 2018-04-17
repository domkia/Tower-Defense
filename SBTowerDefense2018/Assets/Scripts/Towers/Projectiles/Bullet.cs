using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Enemy enemy;
    // Speed of the bullet
    public float Speed = 0.5f;

    public int Damage = 10;

    public float d = 0.2f;
    private Vector3 lastKnownPosition;

    /// <summary>
    /// Sets the bullet's target.
    /// </summary>
    /// <param name="enemy">Reference to enemy</param>
    public void Seek(Enemy enemy)
    {
        this.enemy = enemy;
    }

    private void Update()
    {
        if (enemy != null)
            lastKnownPosition = enemy.transform.position;

        Vector3 dir = lastKnownPosition - transform.position;
        float distanceThisFrame = Speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        float distanceToEnemy = (transform.position - lastKnownPosition).magnitude;
        if (distanceToEnemy <= d)
        {
            if(enemy != null)
                enemy.TakeDamage(Damage);
            Destroy(gameObject);
            return;
        }
    }
}
