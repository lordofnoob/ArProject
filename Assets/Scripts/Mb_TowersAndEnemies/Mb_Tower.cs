using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Mb_Tower : MonoBehaviour
{
    private Sc_Tower towerBaseCharacteristics;
    private TowerCharacteritics towerCharacteristics;
    public ProjectileModifier projectileModifierList;
    public Transform shootProjectilePoint;
    public GameObject ProjectilePrefab;
         
    private List<TileInfo> tileInRange = new List<TileInfo>();
    public TileInfo towerTileInfo;

    private float timer = 0f;
    private List<Mb_Enemy> targets = new List<Mb_Enemy>();

    public void Action()
    {
        if(timer > towerCharacteristics.delayBetweenAttack)
        {
            timer = 0;

            if (targets.Count < towerCharacteristics.numberOfShots)
            {
                SetNewTargets();
            }

            if (targets.Count > 0)
            {
                foreach (Mb_Enemy target in targets)
                {
                    ShootProjectileOnTarget(target);
                }
            }
        }
        timer += Time.fixedDeltaTime;
    }

    public void Init(Sc_Tower towerBaseCharacteristics, GameObject towerTile)
    {
        this.towerBaseCharacteristics = towerBaseCharacteristics;
        towerCharacteristics = towerBaseCharacteristics.towerCharacteristics;
        towerTileInfo = towerTile.GetComponent<TileInfo>();

        foreach(List<TileInfo> list in TileManager.instance.GetTileInfoInRange(towerTileInfo.tileID, towerCharacteristics.range))
        {
            tileInRange.AddRange(list);
        }
        tileInRange = tileInRange.OrderBy(tile => tile.distanceFromGoal).ToList();
    }

    //private List<Mb_Enemy> GetEnnemiesInRange()
    //{
    //    List<Mb_Enemy> ennemiesInRange = new List<Mb_Enemy>();
    //    foreach(TileInfo tile in tileInRange)
    //    {
    //        if(tile.onTileElements.Count > 0)
    //        {
    //            ennemiesInRange.AddRange(tile.onTileElements);
    //        }
    //    }
    //    return ennemiesInRange;
    //}

    private void SetNewTargets()
    {
        targets.Clear();

        foreach(TileInfo tile in tileInRange)
        {
            foreach(Mb_Enemy ennemy in tile.GetClosestEnemies())
            {
                if (targets.Count >= towerCharacteristics.numberOfShots) // On ne sait jamais
                {
                    return;
                }

                if (targets.Contains(ennemy))
                    continue;
                
                targets.Add(ennemy);
            }
        }
    }

    private void ShootProjectileOnTarget(Mb_Enemy target)
    {
        float projectileLifeTime = 1;   // LifeTime Hardwrite pas terrible...
        Mb_Projectile newProjectile = UniversalPool.GetItem("Projectiles").GetComponent<Mb_Projectile>();
        //newProjectile.SetModifier();
        newProjectile.Initialize(shootProjectilePoint.position, target, towerCharacteristics.damages, projectileLifeTime); 

        //Instantiate(ProjectilePrefab, shootProjectilePoint.position, Quaternion.identity);
    }
}

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
