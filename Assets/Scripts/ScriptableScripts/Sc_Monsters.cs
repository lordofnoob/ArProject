using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable/Monster", fileName = "Sc_NewMonsterParameters")]
public class Sc_Monsters : ScriptableObject
{
    public allCharacterisitcs monsterBaseCharacteristics;
}

public enum FamilyType
{
    Savage, Gloutons, Undead, Demon, LightWeight, Scaled
}

[System.Serializable]
public struct allCharacterisitcs
{
    public MonsterSpeed speed;
    public float hitPoint, defense, damageToNexus;
    public FamilyType[] allEnemyFamilies;
}

public enum MonsterSpeed
{
    Quick = 20, 
    Normal = 15,
    Slow = 10,
}
