using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRed : Unit {


    public UnitRed(GameObject unit) : base(unit)
    { }

    protected override void attack(float damage)
    {
        base.attack(damage*Random.Range(1.2f,1.5f));
    }
}
