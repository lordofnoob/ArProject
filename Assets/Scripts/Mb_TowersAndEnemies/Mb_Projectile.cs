using System.Collections;
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
    private float lifeTime = 1;

    private void FixedUpdate()
    {
        if(target.GetUnitState() != UnitState.DEAD)
        {
            lifeTime -= Time.fixedDeltaTime;

            if (lifeTime <= 0)
            {
                transform.position = target.transform.position;
                target.DamageUnit(damage, armorPiercing, fireDamage, slowDuration);
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

    public void SetModifier(ProjectileModifier projectileModifier)
    {
        if ((projectileModifier & ProjectileModifier.Fire)== ProjectileModifier.Fire)
            Fire.gameObject.SetActive(true);
        else
            Fire.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.Ice) == ProjectileModifier.Ice )
            Ice.gameObject.SetActive(true);
        else
            Ice.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.HeavyCaliber) == ProjectileModifier.HeavyCaliber)
            Heavy.gameObject.SetActive(true);
        else
            Heavy.gameObject.SetActive(false);

        if ((projectileModifier & ProjectileModifier.Piercing) == ProjectileModifier.Piercing)
            Piercing.gameObject.SetActive(true);
        else
            Piercing.gameObject.SetActive(false);
    }

    public void Initialize(Vector3 startPosition, float newLifeTime, Mb_Enemy newTarget, float newDamage,  float newArmorPiercing, float newFireDamage, float newSlowDuration)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, TileManager.instance.transform.up);

        lifeTime = newLifeTime;

        target = newTarget;
        
        damage = newDamage;
        armorPiercing = newArmorPiercing;
        fireDamage = newFireDamage;
        slowDuration = newSlowDuration;
        
    }
}
