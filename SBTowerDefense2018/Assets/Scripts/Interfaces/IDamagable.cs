
interface IDamagable<T>
{
    int CurrentHealth { get; set; }
    void TakeDamage(int damage);
    event System.Action<T> OnDeath;
}
