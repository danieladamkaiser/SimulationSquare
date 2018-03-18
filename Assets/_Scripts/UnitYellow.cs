using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitYellow : Unit
{
    private List<Projectile> myProjectiles = new List<Projectile>();
    private GameObject projectile;

    public UnitYellow(GameObject unit, GameObject projectile) : base(unit)
    {
        this.projectile = projectile;
    }

    protected override void checkIfClose(float range)
    {
        base.checkIfClose(range*12);
    }



    //overrode attack method. Creates projectile instead of dealing dmg instantly
    protected override void attack(float damage)
    {
        if (attackCooldown <= 0)
        {
            myProjectiles.Add( new Projectile(Damage, Target, projectile, UnitGO.transform.position));
            attackCooldown = AttackSpeed;
        }
        else attackCooldown -= Time.deltaTime;
        
    }

    protected override void move(float movementSpeed)
    {
        base.move(movementSpeed*1.5f);
    }


    //method used for controling projectiles
    protected override void mySpecial()
    {
        List<Projectile> remainingProjectiles = new List<Projectile>();
        foreach (Projectile proj in myProjectiles)
        {
            if (proj.moveCloserToTarget()) remainingProjectiles.Add(proj);
            
        }
        myProjectiles = remainingProjectiles;
    }
}
