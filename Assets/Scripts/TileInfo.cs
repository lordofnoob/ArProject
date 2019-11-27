using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    OBSTACLE,
    GROUND,
    ATTACKSPAWN,
    DEFENCESPAWN,
    NEXUS
}

public class TileInfo : MonoBehaviour
{
    public int tileID;
    public int distanceFromGoal;
    public TileType tileType;
    public List<Mb_Enemy> onTileElements;
    public Transform[] allSpawnPoint;

    private List<int> closestToGoalNeighbourTiles;
    private System.Random random;
    private bool[] positionIsOccupied;

    private void Awake()
    {
        distanceFromGoal = -1;
        random = GameManager.instance.random;
     
        onTileElements = new List<Mb_Enemy>();
        closestToGoalNeighbourTiles = new List<int>();
    }

    public Vector3 GetOffset(int localPosition)
    {
        if(localPosition < allSpawnPoint.Length && localPosition >= 0)
            return allSpawnPoint[localPosition].transform.localPosition;
        else
            return Vector3.zero;
    }

    public void ResetTile()
    {
        distanceFromGoal = -1;
        if (tileType != TileType.NEXUS);
        tileType = TileType.GROUND;

        onTileElements.Clear();
        closestToGoalNeighbourTiles.Clear();
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

    public List<Mb_Enemy> GetClosestEnemies()
    {
        List<Mb_Enemy> closestEnemies = new List<Mb_Enemy>();

        List<Mb_Enemy> enteringEnemies = new List<Mb_Enemy>();
        List<Mb_Enemy> leavingEnemies = new List<Mb_Enemy>();

        foreach(Mb_Enemy enemy in onTileElements)
        {
            if (enemy.GetFromTile() == tileID)
            {
                leavingEnemies.Add(enemy);
            }
            else if(enemy.GetToTile() == tileID)
            {
                enteringEnemies.Add(enemy);
            }
        }

        leavingEnemies = leavingEnemies.OrderBy(enemy => enemy.GetMovementProgress()).ToList();
        enteringEnemies = enteringEnemies.OrderBy(enemy => enemy.GetMovementProgress()).ToList();

        closestEnemies.AddRange(leavingEnemies);
        closestEnemies.AddRange(enteringEnemies);
        return closestEnemies;
    }

    private void OnTriggerEnter(Collider other)
    {
       Mb_Enemy enteringEnemy = other.gameObject.GetComponent<Mb_Enemy>();
       if (enteringEnemy)    // A modifier?
       {
            if (enteringEnemy.spawnTileID < 0)
                enteringEnemy.spawnTileID = tileID;
            onTileElements.Add(enteringEnemy);
       }
       else 
       {
            Mb_Tower spawningTower = other.gameObject.GetComponent<Mb_Tower>();
            if (spawningTower)
            {
                if (spawningTower.towerTileID < 0)
                    spawningTower.towerTileID = tileID;
            }
       }
    }

    private void OnTriggerExit(Collider other)
    {
        Mb_Enemy leavingEnemy = other.gameObject.GetComponent<Mb_Enemy>();
        if (leavingEnemy)    // A modifier?
        {
            onTileElements.Remove(leavingEnemy);
        }
    }

}
