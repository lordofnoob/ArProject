﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ProjectileModifier
{
    Fire = (1 << 0),
    Ice = (1 << 1),
    Piercing = (1 << 2),
    HeavyCaliber = (1 << 3),

    FireIce = Fire | Ice,
    FirePiercing = Fire | Piercing,
    FireHeavyCaliber = Fire | HeavyCaliber,

    IcePiercing = Ice | Piercing,
    IceHeavyCaliber = Ice | HeavyCaliber,

    PiercingHeavyCaliber = Piercing | HeavyCaliber,

    FireIceHeavyCaliber = Fire | Ice | HeavyCaliber,
    FireIceHeavyPiercing = Fire | Ice | Piercing,

    IceHeavyCaliberPiercing = Ice | HeavyCaliber | Piercing
}

public class Mb_Projectile : MonoBehaviour
{
    public ParticleSystem Fire, Ice, Heavy, Piercing;

    private Mb_Enemy target;

    private float damage;
    private float armorPiercing;
    private float fireDamage;
    private float slowDuration;
    private float lifeTime;

    private void FixedUpdate()
    {
        if(target && target.GetUnitState() != UnitState.DEAD)
        {
            //Debug.LogError(target);

            if (lifeTime <= 0)
            {
                transform.position = target.transform.position;
                target.DamageUnit(damage, armorPiercing, fireDamage, slowDuration);
                UniversalPool.ReturnItem(gameObject, "Projectile");
            }
            else
            {
                Debug.Log((target.transform.position - transform.position) * Time.fixedDeltaTime / lifeTime);
                transform.position += (target.transform.position - transform.position) * Time.fixedDeltaTime / lifeTime;
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, TileManager.instance.transform.up);
            }

            lifeTime -= Time.fixedDeltaTime;
        }
        else
        {
            UniversalPool.ReturnItem(gameObject, "Projectile");
        }
    }

    public void SetModifier(ProjectileModifier projectileModifier)
    {
        if ((projectileModifier & ProjectileModifier.Fire) != 0)
        {
            Fire.gameObject.SetActive(true);
        }
        else
            Fire.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.Ice) != 0) 
            Ice.gameObject.SetActive(true);
        else
            Ice.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.HeavyCaliber) != 0)
            Heavy.gameObject.SetActive(true);
        else
            Heavy.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.Piercing) !=0)
            Piercing.gameObject.SetActive(true);
        else
            Piercing.gameObject.SetActive(false);
    }

    public void Initialize(Vector3 startPosition, float newLifeTime, Mb_Enemy newTarget, float newDamage,  float newArmorPiercing, float newFireDamage, float newSlowDuration)
    {
        target = newTarget;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, TileManager.instance.transform.up);

        lifeTime = newLifeTime;

        damage = newDamage;
        armorPiercing = newArmorPiercing;
        fireDamage = newFireDamage;
        slowDuration = newSlowDuration;
    }
}
