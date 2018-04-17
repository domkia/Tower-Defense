
interface IDamagable<T>
{
    int Health { get; set; }
    void TakeDamage(int damage);
    event System.Action<T> OnDeath;
}
