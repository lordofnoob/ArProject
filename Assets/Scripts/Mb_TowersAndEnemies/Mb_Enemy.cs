using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
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
    private int currentTile;
    private int unitDestination;

    private void Awake()
    {
        monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
        unitDestination = -1;
    }

    public void Action()
    {
        if(unitState == UnitState.DEAD)
        {
            return;
        }
        if(unitState == UnitState.STANDBY)
        {
            unitDestination = TileManager.instance.GetTile(currentTile).GetComponent<TileInfo>().FindNextTile(); ;
        }
        if(unitState == UnitState.MOVING)
        {
            Move();
        }
    }

    private void Move()
    {

    }
}


