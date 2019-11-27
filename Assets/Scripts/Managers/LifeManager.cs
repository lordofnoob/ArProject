using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;
    public GameObject nexusTile;
    public int nexusTileID = 0;
    
    public float playerStartingLife = 100;
    public Image lifeBar;
    private bool playerIsDead = false;
    private float playerRemainingLife;

    private GameObject nexus;

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
        UpdateUi(playerRemainingLife);
        // Changer l'affichage vie

        if (playerRemainingLife <= 0)
        {
            GameManager.instance.GameOver();
        }

      
    }

    void UpdateUi(float lifeRemaining)
    {
        lifeBar.fillAmount = lifeRemaining / playerStartingLife;
    }

    public void NexusInit(int spawnTile)
    {
        nexusTileID = spawnTile;
        TileManager.instance.GetTileInfo(spawnTile).tileType = TileType.NEXUS;
   
        TileManager.instance.SetUnitPosition(Instantiate(nexusTile), spawnTile);
    }
}
