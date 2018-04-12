using UnityEngine;

/// <summary>
/// Remaining ammo indicator for towers.
/// </summary>
public class AmmoIndicator : MonoBehaviour
{
    // If this ratio is reached, the ammo indicator is displayed.
    [Range(0f, 1f)]
    public float LowAmmoRatio;

    // When the tower has low ammo, this sprite is rendered.
    public Sprite LowAmmoSprite;
    // When the tower has no ammo left, this sprite is rendered.
    public Sprite NoAmmoSprite;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Sets up the ammo indicator for this tower.
    /// </summary>
    /// <param name="parent">The parent tower of this ammo indicator.</param>
    public void Setup(Tower parent)
    {
        // We add an offset in the positive Y direction, so we can see the sprite clearly.
        // If we didn't add an offset, the sprite would be embedded in the tower.
        gameObject.transform.position = parent.transform.position + new Vector3(0f, 2f, 0f);
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates this tower's ammo indicator.
    /// </summary>
    /// <param name="ammoLeft">Ammo left in this tower.</param>
    /// <param name="ammoCapacity">Max ammo this tower can hold.</param>
    public void UpdateIndicator(int ammoLeft, int ammoCapacity)
    {
        // No ammo case. The ammo indicator would already be active at this point.
        if (ammoLeft == 0)
            spriteRenderer.sprite = NoAmmoSprite;
        else
        {
            float currentRatio = (float) ammoLeft / ammoCapacity;
            if (currentRatio <= LowAmmoRatio)
            {
                spriteRenderer.sprite = LowAmmoSprite;
                gameObject.SetActive(true);
            }
            else
                gameObject.SetActive(false);
        }
    }
}
