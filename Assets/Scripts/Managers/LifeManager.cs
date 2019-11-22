using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;

    
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
}
