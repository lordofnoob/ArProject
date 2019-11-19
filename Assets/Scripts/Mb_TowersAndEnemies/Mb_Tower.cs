using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Tower : MonoBehaviour
{
    public Sc_Tower towerBaseCharacteristics;
    public TowerCharacteritics towerCharacteristics;


    private void Awake()
    {
        towerCharacteristics = towerBaseCharacteristics.towerCharacteristics;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Modifier
{
    Fire, Ice, Piercing, HeavyCaliber
}

