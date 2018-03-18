using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    private float damage;
    private Unit target;
    private GameObject ProjectileGO;

    public Projectile(float damage, Unit target, GameObject projectilePrefab,Vector2 position)
    {

        this.damage = damage;
        this.target = target;
        ProjectileGO = MonoBehaviour.Instantiate(projectilePrefab,position, Quaternion.identity);
        GameObject.Destroy(ProjectileGO, 0.5f);
    }



    //controlls projectile. If too far moves closer. If close enough deals dmg and destroys projectile. Destroys projectile if target dies
    public bool moveCloserToTarget()
    {
        if (target.UnitGO!= null)
        {
            if (Vector2.Distance(target.UnitGO.transform.position, ProjectileGO.transform.position) > 0.1f)
            {
                ProjectileGO.transform.position = Vector2.MoveTowards(new Vector2(ProjectileGO.transform.position.x, ProjectileGO.transform.position.y), new Vector2(target.UnitGO.transform.position.x, target.UnitGO.transform.position.y), 20 * Time.deltaTime);
                return true;
            }
            else
            {
                target.takeDamage(damage);
                GameObject.Destroy(ProjectileGO);
                return false;
            }
        }
        else GameObject.Destroy(ProjectileGO);
        return false;
    }
}
