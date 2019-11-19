using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sc_Tower : ScriptableObject
{
    public TowerCharacteritics towerCharacteristics;
    public List<Modifier> allTowersModifier;
}

public enum TowerType
{
    Fire, Ice, RapidFire, Piercing, HeavyCaliber, MultiTower
}

public struct TowerCharacteritics
{
    public float range, damages, numberOfShots, delayBetweenAttack, piercingAmount;
    public TowerType[] allTowerFamily;
}
