using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    OBSTACLE,
    GROUND,
    ATTACKSPAWN,
    DEFENCESPAWN
}

public class TileInfo : MonoBehaviour
{
    public int tileID;
    public int distanceFromGoal;
    public TileType tileType;
    public List<GameObject> onTileElements;

    
    private List<int> closestToGoalNeighbourTiles;
    private System.Random random;

    private void Awake()
    {
        distanceFromGoal = -1;
        random = GameManager.instance.random;
    }

    public void AddPath(int tileID)
    {
        closestToGoalNeighbourTiles.Add(tileID);
    }

    public int FindNextTile()
    {
        int randomInt = random.Next(closestToGoalNeighbourTiles.Count);
        return closestToGoalNeighbourTiles[randomInt];
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.GetComponent<Mb_Enemy>())    // A modifier?
       {
            onTileElements.Add(other.gameObject);
       }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Mb_Enemy>())    // A modifier?
        {
            onTileElements.Remove(other.gameObject);
        }
    }
}
