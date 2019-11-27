using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IDLE,
    STANDBY,
    MOVING,
    ATTACKING,
    WAITINGFORDEATH,
    DEAD
}

public class Mb_Enemy : MonoBehaviour
{
    [Header("DebugInitialisation")]
    //public int spawnRow;
    //public int spawnLine;
    public int spawnTileID;

    //[Header("DebugInitialisation")]
    public string itemName;

    [Header("Characteritics")]
    public Sc_Monsters monsterCharacteristics;
    public allCharacterisitcs monsterUpdatedCharacteristics;

    [Header("GraphicPart")]
    public Animator anim;

    //Slow a ajouter!

    public float defaultTimeBeforeDeath = 5f;

    private float remainingHitPoints;
    private float slowRemainingDuration;

    private UnitState unitState;
    private int unitStartingMovementTile;
    private int unitDestinationTile;
    private float movementProgress;

    private float timeBeforeDeath;

    private void Awake()
    {
        //unitDestinationTile = -1;
    }

    private void OnEnable()
    {
        monsterUpdatedCharacteristics = monsterCharacteristics.monsterBaseCharacteristics;
        timeBeforeDeath = defaultTimeBeforeDeath;
        
        //unitState = UnitState.STANDBY;
        //remainingHitPoints = monsterUpdatedCharacteristics.hitPoint;
    }

    //////////////////////////////////////////////////////////      Initialisation      /////////////////////////////////////////////////////////

    public void Init(int tileID)    //Ajouter int Position pour le multiSpawn
    {
        if(PhaseManager.instance.GetCurrentPhase() == Phase.ATTACK)
        {
            TileType tileType = TileManager.instance.GetTileInfo(tileID).tileType;
            if (tileType == TileType.DEFENCESPAWN || tileType == TileType.NEXUS)
                return;

            TileManager.instance.SetUnitPosition(gameObject, tileID);
            //TileManager.instance.GetTileInfo(tileID).tileType = TileType.ATTACKSPAWN;
            unitState = UnitState.STANDBY;
            unitStartingMovementTile = tileID;
            remainingHitPoints = monsterUpdatedCharacteristics.hitPoint;
            PhaseManager.instance.attackers.Add(this);
        }
        else
        {
            UniversalPool.ReturnItem(gameObject, itemName);
        }
    }

    public void InitDebug()
    {
        if(spawnTileID >= 0)
        {
            gameObject.transform.parent = TileManager.instance.transform;
            gameObject.transform.localPosition = TileManager.instance.GetTilePosition(spawnTileID);
            unitState = UnitState.STANDBY;
            remainingHitPoints = monsterUpdatedCharacteristics.hitPoint;
        }
    }

    //public void SetUnitPosition(int spawnTileID)
    //{
    //    gameObject.transform.parent = TileManager.instance.transform;
    //    gameObject.transform.localPosition = TileManager.instance.GetTilePosition(spawnTileID);
    //}

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
            return;
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
                //Debug.Log("MORT!");
                anim.SetTrigger("Died");
                unitState = UnitState.WAITINGFORDEATH;
            }
        }
    }

    public void Action()
    {
        if(unitState == UnitState.DEAD)     // Return ou setactive(false)?
        {
            UniversalPool.ReturnItem(gameObject, itemName);
            return;
        }

        if (unitState == UnitState.WAITINGFORDEATH)
        {
            timeBeforeDeath = -Time.fixedDeltaTime;
            if (timeBeforeDeath < 0)
            {
                unitState = UnitState.DEAD;
                ResetTimeBeforeDeath();
            }

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
            anim.SetTrigger("StartRunning");
            Move();
        }

        if(unitState == UnitState.ATTACKING)
        {
            //Debug.Log("Attacking!");
            Attack();
        }
    }

    
    private void Move()
    {
        if(slowRemainingDuration > 0)
        {
            movementProgress += GetSlowedSpeed((float)monsterUpdatedCharacteristics.speed) / 100f * Time.fixedDeltaTime;
        }
        else
        {
            movementProgress += (float)monsterUpdatedCharacteristics.speed / 100f * Time.fixedDeltaTime;
        }

        //Debug.Log(movementProgress);

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

    private void ResetTimeBeforeDeath()         //Beurk!
    {
        timeBeforeDeath = defaultTimeBeforeDeath;
    }
}


