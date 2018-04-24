using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour {

    private List<SpecialAttack> specialAttack = new List<SpecialAttack>();

	// Use this for initialization
	void Start () {
        ReloadSpecial reload = new ReloadSpecial();
        specialAttack.Add(reload);
        IncreaseDamage damage = new IncreaseDamage();
        specialAttack.Add(damage);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (SpecialAttack SA in specialAttack)
        {
            SA.UpdateCooldown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            specialAttack[0].Do();
        }
	}
    public void doSpecialAttack(int index)
    {
        if (index < 0 || index > specialAttack.Count)
        {
            Debug.Log("Index out of bounds");
            return;
        }
        else
        specialAttack[index].Do();
    }
}
