using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour {

    private List<SpecialAttack> specialAttack = new List<SpecialAttack>(3);
    


	// Use this for initialization
	void Start () {
		
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
}
