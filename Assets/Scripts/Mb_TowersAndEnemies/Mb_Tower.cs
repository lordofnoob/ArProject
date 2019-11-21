using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Tower : MonoBehaviour
{
    private Sc_Tower towerBaseCharacteristics;
    private TowerCharacteritics towerCharacteristics;

    private List<TileInfo> tileInRange = new List<TileInfo>();
    public TileInfo towerTile;

    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(timer > towerCharacteristics.delayBetweenAttack)
        {

        }
    }

    public void Init(Sc_Tower towerBaseCharacteristics, GameObject towerTile)
    {
        this.towerBaseCharacteristics = towerBaseCharacteristics;
        towerCharacteristics = towerBaseCharacteristics.towerCharacteristics;
        this.towerTile = towerTile.GetComponent<TileInfo>();

        /*foreach(List<GameObject> list in TileManager.instance.GetTileInRange(towerTile, towerCharacteristics.range))
        {
            foreach(GameObject tile in list)
            {
                tileInRange.Add(tile.GetComponent<TileInfo>());
            }
        }*/
    }

    private List<Mb_Enemy> GetEnnemiesInRange()
    {
        List<Mb_Enemy> ennemiesInRange = new List<Mb_Enemy>();
        foreach(TileInfo tile in tileInRange)
        {
            //if(tile.)
        }
        return ennemiesInRange;
    }
}

public enum Modifier
{
    Fire, Ice, Piercing, HeavyCaliber
}

