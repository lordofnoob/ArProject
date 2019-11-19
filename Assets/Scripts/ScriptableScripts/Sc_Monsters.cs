using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sc_Monsters : ScriptableObject
{
    public allCharacterisitcs monsterBaseCharacteristics;
}

public enum FamilyType
{
    Savage, Gloutons, Undead, Demon, LightWeight, Scaled
}

public struct allCharacterisitcs
{
    public float speed, hitPoint, defense, damageToNexus;
    public FamilyType[] allEnemyFamilies;
}
