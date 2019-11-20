using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    OBSTACLE,
    GROUND,
    ATTACKSPAWN,
    DEFENCESPAWN
}

public class TileInfo : MonoBehaviour
{
    public int distanceFromGoal;
    public TileType tileType;
}
