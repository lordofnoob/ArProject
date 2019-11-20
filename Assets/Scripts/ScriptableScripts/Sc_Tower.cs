using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable/Tower", fileName = "Sc_NewTowerParameters")]
public class Sc_Tower : ScriptableObject
{
    public TowerCharacteritics towerCharacteristics;
}

public enum TowerType
{
    Fire, Ice, RapidFire, Piercing, HeavyCaliber, MultiTower
}

[System.Serializable]
public struct TowerCharacteritics
{
    public float range, damages, numberOfShots, delayBetweenAttack, piercingAmount;
    public TowerType[] allTowerFamily;
}
