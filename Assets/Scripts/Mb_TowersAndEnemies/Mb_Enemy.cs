using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IDLE,
    STANDBY,
    MOVING,
    DEAD
}

public class Mb_Enemy : MonoBehaviour
{
    [Header("Characteritics")]
    public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    private UnitState unitState;
    private int unitStartingMovementTile;
    private int unitDestinationTile;
    private float movementProgress;

    private void Awake()
    {
        monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
        unitDestinationTile = -1;
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
            unitState = UnitState.MOVING;
            movementProgress = 0;
            transform.localRotation = Quaternion.LookRotation(TileManager.instance.GetTilePosition(unitDestinationTile) - TileManager.instance.GetTilePosition(unitStartingMovementTile), TileManager.instance.transform.up);
        }
        if(unitState == UnitState.MOVING)
        {
            Move();
        }
    }

    private void Move()
    {
        movementProgress += (int)monsterUpdatedCharacteristics.speed / 100 * Time.fixedDeltaTime;

        if(movementProgress >= 1)
        {
            transform.position = TileManager.instance.GetTilePosition(unitDestinationTile);
            unitStartingMovementTile = unitDestinationTile;
            unitState = UnitState.STANDBY;
            return;
        }

        transform.position = TileManager.instance.GetTilePosition(unitStartingMovementTile) + (TileManager.instance.GetTilePosition(unitDestinationTile) - TileManager.instance.GetTilePosition(unitStartingMovementTile)) / movementProgress;
    }
}


