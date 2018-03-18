using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGreen : Unit
{

    public UnitGreen(GameObject unit) : base(unit)
    { }

    public override void takeDamage(float damage)
    {
        base.takeDamage(damage*Random.Range(0.5f,0.8f));
    }

}
