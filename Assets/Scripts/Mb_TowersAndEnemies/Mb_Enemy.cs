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
    [Header("DebugInitialisation")]
    public int spawnRow;
    public int spawnLine;

    [Header("Characteritics")]
    public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    //Slow a ajouter!

    private float remainingHitPoints;

    private UnitState unitState;
    private int unitStartingMovementTile;
    private int unitDestinationTile;
    private float movementProgress;

    private void Awake()
    {
        monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
        unitDestinationTile = -1;
        unitState = UnitState.STANDBY;

        //remainingHitPoints = monsterUpdatedCharacteristics.hitPoint;
    }

    //////////////////////////////////////////////////////////      Initialisation      /////////////////////////////////////////////////////////

    public void Init(int tileID)
    {
        gameObject.transform.parent = TileManager.instance.transform;
        gameObject.transform.localPosition = TileManager.instance.GetTilePosition(TileManager.instance.GetTileID(spawnRow, spawnLine));
        remainingHitPoints = monsterUpdatedCharacteristics.hitPoint;
    }

    public void SetUnitPosition(int spawnTileID)
    {
        gameObject.transform.parent = TileManager.instance.transform;
        gameObject.transform.localPosition = TileManager.instance.GetTilePosition(spawnTileID);
    }

    public void UpdateCharacteristic()
    {
        // Bonus?
    }

    //////////////////////////////////////////////////////////      Getter      /////////////////////////////////////////////////////////
    
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

    ////////////////////////////////////////////////////////////    Behaviour Methods        /////////////////////////////////////////////////////////
    

    //Slow a ajouter!
    public void DamageUnit(float damage)
    {
        if(unitState == UnitState.DEAD)
        {
            //Debug.Log("Pay some respect dude!");
        }
        else
        {
            // Application de la defence!
            remainingHitPoints -= damage;
            if(remainingHitPoints <= 0)
            {
                unitState = UnitState.DEAD;
                // Animation Mort
            }
        }
        
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

            if(TileManager.instance.GetTileInfo(unitDestinationTile).distanceFromGoal == 0)
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
            //Debug.Log("Attacking!");
            Attack();
        }
    }

    //Slow a ajouter!
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

    private void Attack()
    {
        // Animation
        // Attendre pour une certaine durée
        //LifeManager.instance.DamagePlayer(monsterUpdatedCharacteristics.damageToNexus);
        //unitState = UnitState.DEAD;
        // Animation de mort
    }
}


