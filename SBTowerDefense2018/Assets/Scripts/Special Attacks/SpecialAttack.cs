using System;
using UnityEngine;

public abstract class SpecialAttack : ScriptableObject
{
    public string title;
    public Sprite icon;
    public string description;
    public float cooldown;
    // Sound that is played when an ability is ready and is activated by the player.
    public AudioClip AbilityActivatedSFX;

    protected float timer = 0;
    protected bool isReady = true;

    public abstract void Do();

    public void UpdateCooldown()
    {
        if (isReady)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isReady = true;
            UISoundPlayer.Instance.PlayAbilityReadySound();
        }
    }
    
    public bool IsReady
    { get { return isReady; } }

    public float CooldownProgress
    {
        get { return timer / cooldown; }
    }

    public void Reset()
    {
        isReady = true;
        timer = 0f;
    }
}
