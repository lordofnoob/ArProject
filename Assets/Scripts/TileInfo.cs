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
    DEFENCESPAWN
}

public class TileInfo : MonoBehaviour
{
    public int tileID;
    public int distanceFromGoal;
    public TileType tileType;
    public List<Mb_Enemy> onTileElements;

    private List<int> closestToGoalNeighbourTiles;
    private System.Random random;

    private void Awake()
    {
        distanceFromGoal = -1;
        //random = GameManager.instance.random;
        random = TestGameManager.instance.random;
        onTileElements = new List<Mb_Enemy>();
        closestToGoalNeighbourTiles = new List<int>();
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
            Debug.Log("Entering " + tileID);
            onTileElements.Add(enteringEnemy);
       }
    }

    private void OnTriggerExit(Collider other)
    {
        Mb_Enemy leavingEnemy = other.gameObject.GetComponent<Mb_Enemy>();
        if (leavingEnemy)    // A modifier?
        {
            Debug.Log("Leaving " + tileID);
            onTileElements.Remove(leavingEnemy);
        }
    }
}
