using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit 
{
    public float Health { get; protected set; }
    public float Damage { get;  }
    public float MovementSpeed { get;  }
    public float AttackSpeed { get;  }
    public Unit Target { get; private set; }
    public GameObject UnitGO { get; private set; }
    
    public float maxHealth { get; private set; }

    protected float range = 0.3f;
    protected float attackCooldown =0;

    protected GameObject myHealthbar;

    protected Unit(GameObject spawnedUnit)
    {
        Health = Random.Range(5f, 10f);
        Damage =   Random.Range(1f, 1.5f);
        MovementSpeed = Random.Range(2f, 2.25f);
        AttackSpeed = Random.Range(0.25f, 0.5f);
        maxHealth = Health;

        UnitGO = MonoBehaviour.Instantiate(spawnedUnit);

    }



    public void Update()
    {
        mySpecial();
        checkIfClose(range);
    }

    

    protected virtual void mySpecial()
    {
        //method for unique abilities like regen of blue unit
    }

    protected virtual void checkIfClose(float range)
    {

        //Get Closer if too far else fight. Vector2.distance just for simplicity
        if (Target != null)
        {
            if (Target.UnitGO != null)
            {
                if (Vector2.Distance(Target.UnitGO.transform.position, UnitGO.transform.position) > range) move(MovementSpeed); else attack(Damage);
            }
            else Target = null;
        }
    }

    protected virtual void move(float movementSpeed)
    {
        //move me closer to my targeted enemy
        UnitGO.transform.position = Vector2.MoveTowards(new Vector2( UnitGO.transform.position.x,UnitGO.transform.position.y), new Vector2(Target.UnitGO.transform.position.x,Target.UnitGO.transform.position.y), movementSpeed * Time.deltaTime);
    }



    protected virtual void attack(float damage)
    {
        //Attack if ready or wait till cooldown
        if (attackCooldown <= 0)
        {
            Target.takeDamage(damage);
            attackCooldown = AttackSpeed;
        }
        else attackCooldown -= Time.deltaTime;
    }

    public virtual void takeDamage(float damage)
    {
        
        Health -= damage;
    }

    public virtual void setTarget(Unit target)
    {
        //set my new target enemy
        Target = target;
    }

    public void setPosition(Vector2 position)
    {
        //set my position
        UnitGO.transform.position = position;
    }

}
