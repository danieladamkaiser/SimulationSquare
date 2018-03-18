using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar
{
    public GameObject healthbarBG { get; private set; }

    private Unit owner;
    private GameObject healthbar;


    //creates healthabr instance
    public Healthbar(Unit owner, GameObject healthbarPrefab)
    {
        this.owner = owner;

        healthbarBG = MonoBehaviour.Instantiate(healthbarPrefab);
        healthbar = healthbarBG.transform.Find("Healthbar").gameObject;

    }

    //controls healthbar position and scale to show current health of unit
    public void Update()
    {
        if (owner.Health > 0)
        {
            healthbarBG.transform.position = owner.UnitGO.transform.position;
            healthbar.transform.localScale = new Vector3(owner.Health / owner.maxHealth*0.9f, 0.6f, 1);
        }
        else GameObject.Destroy(healthbarBG);

    }
}

