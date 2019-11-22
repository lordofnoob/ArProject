using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IDLE,
    STANDBY,
    MOVING,
    ATTACKING,
    DEAD,
    WAITINGFORDEATH
}

public class Mb_Enemy : MonoBehaviour
{
    [Header("DebugInitialisation")]
    public int spawnRow;
    public int spawnLine;

    //[Header("DebugInitialisation")]
    public string itemName;

    [Header("Characteritics")]
    public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    public float defaultTimeBeforeDeath = 5;

    private float remainingHitPoints;
    private float slowRemainingDuration;

    private UnitState unitState;
    private int unitStartingMovementTile;
    private int unitDestinationTile;
    private float movementProgress;

    private float timeBeforeDeath;

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
    public void DamageUnit(float damage, float armorPercing, float trueDamage, float slowDuration)
    {
        if(unitState == UnitState.DEAD)
        {
            //Debug.Log("Pay some respect dude!");
        }
        else
        {
            remainingHitPoints -= damage * (1 - monsterUpdatedCharacteristics.defense + armorPercing) + trueDamage;

            if(slowDuration> slowRemainingDuration)
            {
                slowRemainingDuration = slowDuration;
            }

            if(remainingHitPoints <= 0)
            {
                Debug.Log("MORT!");
                anim.SetTrigger("Died");
                unitState = UnitState.WAITINGFORDEATH;
            }
        }
        
    }

    public void Action()
    {
        if(unitState == UnitState.DEAD)
        {
            gameObject.SetActive(false);
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
            anim.SetTrigger("StartRunnig");
            Move();
        }

        if(unitState == UnitState.ATTACKING)
        {
            Attack();
        }

        if(unitState == UnitState.WAITINGFORDEATH)
        {
            timeBeforeDeath = -Time.fixedDeltaTime;
            if(timeBeforeDeath < 0)
            {
                unitState = UnitState.DEAD;
                ResetTimeBeforeDeath();
            }
        }
    }

    //Slow a ajouter!
    private void Move()
    {
        if(slowRemainingDuration > 0)
        {
            movementProgress += GetSlowedSpeed((float)monsterUpdatedCharacteristics.speed) / 100f * Time.fixedDeltaTime;
            slowRemainingDuration -= Time.fixedDeltaTime;
        }
        else
        {
            movementProgress += (float)monsterUpdatedCharacteristics.speed / 100f * Time.fixedDeltaTime;
        }
        

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
        anim.SetTrigger("ReachedNexus");
        LifeManager.instance.DamagePlayer(monsterUpdatedCharacteristics.damageToNexus);
        unitState = UnitState.WAITINGFORDEATH;
    }

    private float GetSlowedSpeed(float speed)
    {
        return speed * 4f / 5f;

    }

    private void ResetTimeBeforeDeath()
    {
        timeBeforeDeath = defaultTimeBeforeDeath;
    }
}


