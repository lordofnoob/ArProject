using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Projectile : MonoBehaviour
{
    public ParticleSystem Fire, Ice, Heavy, Piercing;

    private Mb_Enemy target;
    private float damage;
    private float lifeTime = 1;

    private void FixedUpdate()
    {
        if(target.GetUnitState() != UnitState.DEAD)
        {
            lifeTime -= Time.fixedDeltaTime;

            if (lifeTime <= 0)
            {
                transform.position = target.transform.position;
                target.DamageUnit(damage);
                UniversalPool.ReturnItem(gameObject, "Projectiles");
            }
            else
            {
                transform.position += (target.transform.position - transform.position) * Time.fixedDeltaTime / lifeTime;
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, TileManager.instance.transform.up);
            }
        }
        else
        {
            UniversalPool.ReturnItem(gameObject, "Projectiles");
        }
    }

    public void SetModifier(bool fire, bool ice, bool heavy, bool piercing)
    {
        Fire.gameObject.SetActive(fire);
        Ice.gameObject.SetActive(ice);
        Heavy.gameObject.SetActive(heavy);
        Piercing.gameObject.SetActive(piercing);
    }

    public void Initialize(Vector3 startPosition, Mb_Enemy newTarget, float newDamage, float newLifeTime )
    {
        target = newTarget;
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, TileManager.instance.transform.up);
        damage = newDamage;
        lifeTime = newLifeTime;
    }
}
