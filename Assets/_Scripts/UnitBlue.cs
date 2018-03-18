using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlue : Unit {


    public UnitBlue(GameObject unit) : base(unit)
    { }

    protected override void mySpecial()
    {
        if (Health<maxHealth)
        {
            Health += maxHealth / 10 * Time.deltaTime;
            
        }
        if (Health > maxHealth)
            Health = maxHealth;
    }
}
