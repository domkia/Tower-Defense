using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base Enemy class
/// </summary>
public abstract class Enemy : MonoBehaviour, IDamagable<Enemy>, ISelectable
{

    public HexTile currentlyOn { get; set; }                    //Tile this enemy is currently on
    public int Damage { get; set; }                             //Every enemy has damage?
    public GameObject bloodPrefab;
    //IDamagable implementation
    public event Action<Enemy> OnDeath;

    public int CurrentHealth { get; set; }

    //Enemy states
    public abstract void Idle();                                //Do nothing
    public abstract void Move(HexTile startTile);               //Just move along given path
    public abstract void Attack();

    protected IEnemyState currentState;
    public float Speed { get; protected set; }
    public int moneyReward = 10;

    public Color SelectionColor { get { return Color.red; } }
    public Animator animator;
    public MeshRenderer mesh;
    public float fadeOutTime = 3f;

    protected Healthbar healthBar;

    protected PlayEnemySFX soundEffectPlayer;

    protected void Awake()
    {
        GameManager.OnGameOver += Idle;
        healthBar = GetComponent<Healthbar>();
        soundEffectPlayer = GetComponentInChildren<PlayEnemySFX>();
        animator = GetComponentInChildren<Animator>();
        Idle();                                                 //Initial state is Idle
    }

    protected void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        DamageEffect();

        if (CurrentHealth <= 0)
        {
            GiveReward(moneyReward);
            Die();
        }
    }

    protected virtual void DamageEffect()
    {
        animator.SetFloat("RandomHit", UnityEngine.Random.value);
        animator.SetTrigger("Hit");
    }

    protected void Die()
    {
        PlayerStats.Instance.EnemyKilled();
        if (OnDeath != null)
            OnDeath(this);
        healthBar.Vissible = false;

        StartCoroutine("DeathEffect");
    }

    /// <summary>
    /// Play death sound, animation, fade out etc.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeathEffect()
    {
        currentState = null;
        soundEffectPlayer.Play(SoundType.EnemyDeath);
        animator.SetTrigger("Die");

        //Fade here
        yield return new WaitForSeconds(fadeOutTime);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= Idle;
    }

    protected void GiveReward(int moneyReward)
    {
        PlayerStats.Instance.ChangeMoney(moneyReward);
    }
    
    public float startSpeed = 0.1f;
    public float freeze = 0;
    public float freezeDuration = 5;
    public float enemySpeed; 

    public void Freeze()
    {
        enemySpeed = Speed; // save start enemy speed
        Speed = startSpeed * freeze;
        animator.speed = 0f;
        Invoke("ResetFreeze", freezeDuration);
    }
    
    private void ResetFreeze()
    {
        animator.speed = 1f;
        Speed = enemySpeed;
    }
}