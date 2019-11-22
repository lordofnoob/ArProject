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
    public float speed, hitPoint, defense, damageToNexus;
    public FamilyType[] allEnemyFamilies;

}


