using System;

/// <summary>
/// Base Enemy class
/// </summary>
abstract class Enemy
{
    public abstract event Action OnEnemyDied;
    public abstract void TakeDamage();
}
