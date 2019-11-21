using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Mb_Tower : MonoBehaviour
{
    private Sc_Tower towerBaseCharacteristics;
    private TowerCharacteritics towerCharacteristics;

    private List<TileInfo> tileInRange = new List<TileInfo>();
    public TileInfo towerTileInfo;

    private float timer = 0f;
    private List<Mb_Enemy> targets = new List<Mb_Enemy>();

    public void Action()
    {
        if(timer > towerCharacteristics.delayBetweenAttack)
        {
            timer = 0;

            if(targets.Count > 0)
            {
                if (targets.Count < towerCharacteristics.numberOfShots)
                {
                    SetTarget();
                }

                foreach (Mb_Enemy target in targets)
                {
                    //ShOOT TARGET
                }
            }
        }
        timer += Time.deltaTime;
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

    private List<Mb_Enemy> GetEnnemiesInRange()
    {
        List<Mb_Enemy> ennemiesInRange = new List<Mb_Enemy>();
        foreach(TileInfo tile in tileInRange)
        {
            if(tile.onTileElements.Count > 0)
            {
                ennemiesInRange.AddRange(tile.onTileElements);
            }
        }
        return ennemiesInRange;
    }

    private void SetTarget()
    {
        foreach(TileInfo tile in tileInRange)
        {
            foreach(Mb_Enemy ennemy in tile.GetClosestEnnemies())
            {
                targets.Add(ennemy);

                if(targets.Count == towerCharacteristics.numberOfShots)
                {
                    return;
                }
            }
        }
    }
}

public enum Modifier
{
    Fire, Ice, Piercing, HeavyCaliber
}

