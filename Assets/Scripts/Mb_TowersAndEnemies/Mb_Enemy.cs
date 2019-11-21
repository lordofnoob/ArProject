using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IDLE,
    STANDBY,
    MOVING,
    ATTACKING,
    DEAD
}

public class Mb_Enemy : MonoBehaviour
{
    [Header("Initialisation")]
    public int spawnRow;
    public int spawnLine;

    [Header("Characteritics")]
    //public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    private UnitState unitState;
    private int unitStartingMovementTile;
    private int unitDestinationTile;
    private float movementProgress;

    private void Awake()
    {
        //monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
        unitDestinationTile = -1;
        unitState = UnitState.STANDBY;
    }

    public void InitializePosition()
    {
        gameObject.transform.parent = TileManager.instance.transform;
        gameObject.transform.localPosition = TileManager.instance.GetTilePosition(TileManager.instance.GetTileID(spawnRow, spawnLine));
    }

    public UnitState GetUnitState()
    {
        return unitState;
    }

    public int GetFromTile()
    {
        return unitStartingMovementTile;
    }

    public int GetToTile()
    {
        return unitDestinationTile;
    }

    public float GetMovementProgress()
    {
        return movementProgress;
    }

    public void Action()
    {
        if(unitState == UnitState.DEAD)
        {
            return;
        }
        if (unitState == UnitState.STANDBY)
        {
            unitDestinationTile = TileManager.instance.GetTileInfo(unitStartingMovementTile).FindNextTile();
            Debug.Log(unitDestinationTile);
            Debug.Log(TileManager.instance.GetTileInfo(unitDestinationTile).distanceFromGoal);
            if(unitDestinationTile == 0)
            {
                unitState = UnitState.ATTACKING;
            }
            else
            {
                unitState = UnitState.MOVING;
                movementProgress = 0;
                transform.localRotation = Quaternion.LookRotation(TileManager.instance.GetTilePosition(unitDestinationTile) - TileManager.instance.GetTilePosition(unitStartingMovementTile), TileManager.instance.transform.up);
            }
        }
        if(unitState == UnitState.MOVING)
        {
            Move();
        }
        if(unitState == UnitState.ATTACKING)
        {
            Debug.Log("Attacking!");
        }
    }

    private void Move()
    {
        movementProgress += (float)monsterUpdatedCharacteristics.speed / 100f * Time.fixedDeltaTime;

        if(movementProgress >= 1)
        {
            transform.localPosition = TileManager.instance.GetTilePosition(unitDestinationTile);
            unitStartingMovementTile = unitDestinationTile;
            unitState = UnitState.STANDBY;
            return;
        }

        transform.localPosition = TileManager.instance.GetTilePosition(unitStartingMovementTile) + ( TileManager.instance.GetTilePosition(unitDestinationTile) - TileManager.instance.GetTilePosition(unitStartingMovementTile) ) * movementProgress;
    }

    
}


