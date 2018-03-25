using System.Collections.Generic;
using UnityEngine;

public class UpdateAmmoIndicators : MonoBehaviour {

    // Sprite to be drawn when a tower has no ammo left.
    public Sprite NoAmmoIndicator;
    // Sprite to be drawn when a tower has low ammo.
    public Sprite LowAmmoIndicator;
    // Percentage of bullets left at which a tower is considered to have low ammo.
    [Range(0, 100)] public int LowAmmoPercentage;
    // Game object which has towers as its children.
    public GameObject TowerGroup;

    private Dictionary<Tower, GameObject> ammoIndicators;

    private void Start()
    {
        ammoIndicators = new Dictionary<Tower, GameObject>();
        Tower[] towers = TowerGroup.GetComponentsInChildren<Tower>();
        foreach(var tower in towers)
        {
            GameObject indicator = new GameObject("AmmoIndicator");

            indicator.transform.parent = this.transform;
            // We add an offset in the positive Y direction, so we can see the sprite clearly.
            // If we didn't add an offset, the sprite would be embedded in the tower.
            indicator.transform.position = tower.transform.position + new Vector3(0, 2f, 0);
            indicator.SetActive(false);
            // We add a SpriteRenderer component as we will need to render sprites.
            indicator.AddComponent<SpriteRenderer>();

            ammoIndicators.Add(tower, indicator);
        }
    }

    // TODO: switch to callback system (OnAmmoChanged?)
    private void Update()
    {
        // We iterate through all our towers.
        foreach(var towerIndicatorPair in ammoIndicators)
        {
            Tower tower = towerIndicatorPair.Key;
            GameObject indicatorGO = towerIndicatorPair.Value;
            SpriteRenderer renderer = indicatorGO.GetComponent<SpriteRenderer>();

            // No ammo case.
            if (tower.BulletsLeft == 0)
            {
                renderer.sprite = NoAmmoIndicator;
                indicatorGO.SetActive(true);
            }
            // Low ammo case.
            else if (tower.BulletsLeft <= (int)((LowAmmoPercentage / 100.0f) * tower.AmmoCapacity))
            {
                renderer.sprite = LowAmmoIndicator;
                indicatorGO.SetActive(true);
            }
            // Otherwise, the tower has plenty of ammo and we can stop drawing the sprite.
            else
                indicatorGO.SetActive(false);
        }
    }

}
