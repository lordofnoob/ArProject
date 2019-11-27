using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;
    public string nexusItemName;
    public int nexusTileID = 0;
    
    public float playerStartingLife = 100;
    
    private bool playerIsDead = false;
    private float playerRemainingLife;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        playerRemainingLife = playerStartingLife;
    }

    public void SetStartingLife(float newStartingLife)
    {
        playerStartingLife = newStartingLife;
    }

    public void DamagePlayer(float damage)
    {
        // Animation/FX?

        playerRemainingLife -= damage;
        
        // Changer l'affichage vie

        if(playerRemainingLife <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void NexusInit(int spawnTile)
    {
        nexusTileID = spawnTile;
        TileManager.instance.GetTileInfo(spawnTile).tileType = TileType.NEXUS;
        TileManager.instance.SetUnitPosition(UniversalPool.GetItem(nexusItemName), spawnTile);
    }
}
