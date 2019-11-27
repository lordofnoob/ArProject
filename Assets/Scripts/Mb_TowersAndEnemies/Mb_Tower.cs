﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mb_Tower : MonoBehaviour
{
    public Sc_Tower towerBaseCharacteristics;
    public TowerCharacteritics towerCharacteristics;
    public ProjectileModifier projectileModifierList;
    public Transform shootProjectilePoint;
    public GameObject ProjectilePrefab;

    public float projectileLifeTime = 2;   // LifeTime Hardwrite pas terrible...

    private List<TileInfo> tileInRange = new List<TileInfo>();
    public int towerTileID = -1;
    public string itemName;

    private float timer = 0f;
    private List<Mb_Enemy> targets = new List<Mb_Enemy>();


    private void Awake()
    {
        towerCharacteristics = towerBaseCharacteristics.towerCharacteristics;
    }

    public void InitDebug()
    {
        TileManager.instance.SetUnitPosition(gameObject, towerTileID);

        foreach (List<TileInfo> list in TileManager.instance.GetTileInfoInRange(towerTileID, towerCharacteristics.range))
        {
            tileInRange.AddRange(list);
        }
        tileInRange = tileInRange.OrderBy(tile => tile.distanceFromGoal).ToList();
    }

    public void Init(int spawnTile)
    {
        if(PhaseManager.instance.GetCurrentPhase() == Phase.DEFENCE)        // Ajouter le check de spawn?
        {
            towerTileID = spawnTile;
            TileManager.instance.SetUnitPosition(gameObject, towerTileID);

            foreach (List<TileInfo> list in TileManager.instance.GetTileInfoInRange(towerTileID, towerCharacteristics.range))
            {
              //  Debug.Log("TileCount" + tileInRange.Count);
                tileInRange.AddRange(list);
            }
            tileInRange = tileInRange.OrderBy(tile => tile.distanceFromGoal).ToList();

            TileManager.instance.GetTileInfo(spawnTile).tileType = TileType.DEFENCESPAWN;
            PhaseManager.instance.defenders.Add(this);
        }
        else
        {
            UniversalPool.ReturnItem(gameObject, itemName);
        }
    }

    //public void SetUnitPosition(int spawnTileID)
    //{
    //    gameObject.transform.parent = TileManager.instance.transform;
    //    gameObject.transform.localPosition = TileManager.instance.GetTilePosition(spawnTileID);
    //}

    public void Action()
    {

        if (timer > towerCharacteristics.delayBetweenAttack)
        {
            timer = 0;

            if (targets.Count < towerCharacteristics.numberOfShots)
            {
                SetNewTargets();
            }

            for (int i = 0; i < targets.Count; i++)
            {
                Mb_Enemy target = targets[i];
                if (IsInRange(target))
                {
                    ShootProjectileOnTarget(target);
                }
                else
                {  targets.RemoveAt(i--);
                    SetNewTargets();
                }
            }
        }
        timer += Time.fixedDeltaTime;
    }

    private void SetNewTargets()
    {
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
                
                if (ennemy.GetUnitState() != UnitState.DEAD && ennemy.GetUnitState() != UnitState.WAITINGFORDEATH)
                {
                    targets.Add(ennemy);
                   
                }
            }
        }
    }

    public bool IsInRange(Mb_Enemy ennemy)
    {
        foreach (TileInfo tile in tileInRange)
        {
            if (tile.GetClosestEnemies().Contains(ennemy))
            {
                if (ennemy.GetUnitState() != UnitState.DEAD && ennemy.GetUnitState() != UnitState.WAITINGFORDEATH)
                    return true;
            }
        }
        return false;
    }

    public void SetupParticleTrail()
    {
        if (towerCharacteristics.fireDamages > 0)
            projectileModifierList |= ProjectileModifier.Fire;
        if (towerCharacteristics.slowDuration > 0)
            projectileModifierList |= ProjectileModifier.Ice;
        if (towerCharacteristics.piercingAmount > 0)
            projectileModifierList |= ProjectileModifier.Piercing;
        if (towerCharacteristics.damages > 20)
            projectileModifierList |= ProjectileModifier.HeavyCaliber;
    }

    private void ShootProjectileOnTarget(Mb_Enemy target)
    {
        Debug.Log("PewPierPew!");
        Mb_Projectile newProjectile = UniversalPool.GetItem("Projectile").GetComponent<Mb_Projectile>();

        newProjectile.SetModifier(projectileModifierList);
        newProjectile.Initialize(shootProjectilePoint.position, projectileLifeTime, target, towerCharacteristics.damages, towerCharacteristics.piercingAmount, towerCharacteristics.fireDamages, towerCharacteristics.slowDuration);
    }
}


