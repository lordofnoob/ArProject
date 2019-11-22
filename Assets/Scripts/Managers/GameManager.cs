using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public System.Random random;

    private PhaseManager phaseManager;
    
    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        } 
        
        instance = this;
        phaseManager = new PhaseManager();
        random = new System.Random();
    }

    public void GameOver()
    {
        // Afficher Ecran de Game Over
        // Afficher recap?
        // Proposer de relancer une partie
    }

    private void SetGameSettings()
    {
        // Util?
    }

    private void StartGameLoop()
    {
        phaseManager.Initiate();
    }
}
