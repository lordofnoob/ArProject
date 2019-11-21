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

    private void Start()
    {
        
    }

    private void SetGameSettings()
    {

    }

    private void StartGameLoop()
    {
        phaseManager.Initiate();
    }
}
