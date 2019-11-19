using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public GameManager instance;
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
