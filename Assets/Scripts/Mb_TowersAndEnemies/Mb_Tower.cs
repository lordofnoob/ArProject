﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mb_Tower : MonoBehaviour
{
    [SerializeField] Sc_Tower towerBaseCharacteristics;
    private TowerCharacteritics towerCharacteristics;
    public ProjectileModifier projectileModifierList;
    public Transform shootProjectilePoint;
    public GameObject ProjectilePrefab;
         
    private List<TileInfo> tileInRange = new List<TileInfo>();
    public int towerTileID;

    private float timer = 0f;
    private List<Mb_Enemy> targets = new List<Mb_Enemy>();

    public void Action()
    {
        timer = towerCharacteristics.delayBetweenAttack + 1;

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
                if(isInRange(target))
                {
                    ShootProjectileOnTarget(target);
                }
                else
                {
                    Debug.Log("Target out of range! " + target);
                    targets.RemoveAt(i--);
                    SetNewTargets();
                }
            }
        }
        timer += Time.fixedDeltaTime;
    }

    public void Init(int spawnTile)
    {
        towerTileID = spawnTile;
        SetPosition(towerTileID);

        towerCharacteristics = towerBaseCharacteristics.towerCharacteristics;
        
        foreach(List<TileInfo> list in TileManager.instance.GetTileInfoInRange(towerTileID, towerCharacteristics.range))
        {
            tileInRange.AddRange(list);
        }
        tileInRange = tileInRange.OrderBy(tile => tile.distanceFromGoal).ToList();
    }

    private void SetPosition(int tileID)
    {
        gameObject.transform.parent = TileManager.instance.transform;
        transform.localPosition = TileManager.instance.GetTilePosition(tileID);
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
                
                targets.Add(ennemy);
                Debug.Log("Target acquired! " + ennemy);
            }
        }
    }

    public bool isInRange(Mb_Enemy ennemy)
    {
        foreach (TileInfo tile in tileInRange)
        {
            if (tile.GetClosestEnemies().Contains(ennemy))
                return true;
        }
        return false;
    }

    private void ShootProjectileOnTarget(Mb_Enemy target)
    {
        Debug.Log("PewPierPew!");
        float projectileLifeTime = 1;   // LifeTime Hardwrite pas terrible...
        Mb_Projectile newProjectile = UniversalPool.GetItem("Projectiles").GetComponent<Mb_Projectile>();
        newProjectile.SetModifier(projectileModifierList);
        newProjectile.Initialize(shootProjectilePoint.position, projectileLifeTime, target, towerCharacteristics.damages, towerCharacteristics.piercingAmount, towerCharacteristics.fireDamages, towerCharacteristics.slowDuration); 

        //Instantiate(ProjectilePrefab, shootProjectilePoint.position, Quaternion.identity);
    }
}


