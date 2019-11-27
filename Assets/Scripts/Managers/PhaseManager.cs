using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    INIT,
    DEFENCE,
    ATTACK,
    COMPATIBILITY,
    RESOLUTION
}

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public float farness = 1;
    public Transform tileGridTransform;

    private Phase currentPhase;
    private Coroutine currentPhaseCoroutine;

    public List<Mb_Enemy> attackers;
    public List<Mb_Tower> defenders;

    public void Initiate()
    {
        ScanManager.instance.ResetScan();
        currentPhaseCoroutine = StartCoroutine(InitPhase());
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }

    public void SetCurrentPhase(Phase nextPhase)
    {
        ScanManager.instance.ChangePhase(nextPhase);
        currentPhase = nextPhase;
    }

    public IEnumerator InitPhase()
    {
        SetCurrentPhase(Phase.INIT);
        
        TileManager.instance.InstanciateGrid();
        //TileManager.instance.gameObject.SetActive(false);
        tileGridTransform.position = Vector3.forward * farness;
        tileGridTransform.rotation = Quaternion.Euler(-90, 0, 0);
        TileManager.instance.SetTileGridTransform(tileGridTransform);

        //Placer le nexus!
        bool isClicked = false;
        while(!isClicked)
        {
            // Verifier si le bouton/ecran a été click 
            isClicked = ScanManager.instance.initValidate;
            yield return 0;
        }

        Debug.Log("Defence Phase");
        currentPhaseCoroutine = StartCoroutine(DefencePhase());
    }

    private IEnumerator DefencePhase()
    {
        SetCurrentPhase(Phase.DEFENCE);

        bool isValidate = false;
        while(!isValidate)
        {
            // Check/Scan les cartes
            isValidate = ScanManager.instance.defendersValidate;
            // Ajouter les units correspondante à la liste Defenders
            // + Ajouter/Enlever les units à/de la scène
            yield return 0;
        }

        TileManager.instance.SetPathFinding(LifeManager.instance.nexusTileID); // A Deplacer Peut etre...
        
        Debug.Log("Attack Phase");
        currentPhaseCoroutine = StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase()
    {
        SetCurrentPhase(Phase.ATTACK);

        bool isValidate = false;
        while (!isValidate)
        {
            // Check les cartes
            isValidate = ScanManager.instance.attackersValidate;
            // Ajouter/Enlever les units correspondante à la liste Attackers
            // +Ajouter/Enlever les units à/de la scène
            yield return 0;
        }

        Debug.Log("Check Compatibilité");
        currentPhaseCoroutine = StartCoroutine(CompatibilityPhase());
    }

    private IEnumerator CompatibilityPhase()
    {
        SetCurrentPhase(Phase.COMPATIBILITY);
        GetAllFamilies();
        // Afficher UI
        // Check Compatibility
        // Afficher UI (Bonus) 

        bool isClicked = false;
        while (!isClicked)
        {
            // Verifier si le bouton/ecran a été click
            yield return 0;

            isClicked = true; //Pour Debug
        }

        // Desafficher l'UI
        Debug.Log("Debut resolution");
        currentPhaseCoroutine = StartCoroutine(ResolutionPhase());
    }

    private IEnumerator ResolutionPhase()
    {
        SetCurrentPhase(Phase.RESOLUTION);

        // Afficher l'UI?

        bool isResolved = false;
        while(!isResolved)
        {
            ExecuteActions();
            isResolved = CheckResolutionState();
            yield return new WaitForFixedUpdate();
        }

        // Afficher le résultat

        //bool isClicked = false;
        //while(!isClicked)
        //{
        //    // Attendre que le joueur veuille start le prochain round (click bouton/ecran)
        //    yield return 0;
        //}

        // Desafficher l'UI


        ResetBoard();
        ScanManager.instance.ResetScan();
        currentPhaseCoroutine = StartCoroutine(DefencePhase());
    }

    private void ExecuteActions()
    {
        foreach(Mb_Enemy attacker in attackers)
        {
            attacker.Action();
        }

        foreach(Mb_Tower defender in defenders)
        {
            defender.Action();
        }
    }

    private bool CheckResolutionState()
    {
        foreach(Mb_Enemy enemy in attackers)
        {
            if (enemy && enemy.gameObject.activeSelf)
            {
                return false;
            }
        }
 
        return true;
    }

    private void ResetBoard()
    {
        ResetTileGrid();
        ClearDefeneders();
        ClearAttackers();
    }

    private void ClearDefeneders()
    {
        foreach(Mb_Tower defender in defenders)
        {
            defender.ReturnToPool();
        }
        defenders.Clear();
    }

    private void ClearAttackers()
    {
        foreach(Mb_Enemy attacker in attackers)
        {
            if(attacker)
            {
                Debug.Log(attacker);
                attacker.ReturnToPool();
            } 
        }
        attackers.Clear();
    }

    private void ResetTileGrid()
    {
        TileManager.instance.ResetTiles();
    }

    void GetAllFamilies()
    {
        #region EnemiesSetup
        FamilyMonsterCount familyCounting;
        familyCounting.demonCount = 0;
        familyCounting.gloutonCount = 0;
        familyCounting.lightweightCount = 0;
        familyCounting.scaledCount = 0;
        familyCounting.savageCount = 0;
        familyCounting.undeadCount = 0;

        List<Mb_Enemy> allDiferentMonsters = new List<Mb_Enemy>();
        for (int i = 0; i < attackers.Count; i++)
        {
            bool alreadyThere = false;

            for (int y =0; y < allDiferentMonsters.Count; y++)
            {
                if (attackers[i].monsterCharacteristics == allDiferentMonsters[y].monsterCharacteristics)
                    alreadyThere = true;
            }

            if (alreadyThere == false)
                allDiferentMonsters.Add(attackers[i]);

        }

        for (int i = 0; i < allDiferentMonsters.Count; i++)
        {
            for (int y = 0; y < allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies.Length; y++)
            {
                if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Demon)
                    familyCounting.demonCount += 1;
                else if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Gloutons)
                    familyCounting.gloutonCount += 1;
                else if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.LightWeight)
                    familyCounting.lightweightCount += 1;
                else if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Scaled)
                    familyCounting.scaledCount += 1;
                else if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Savage)
                    familyCounting.savageCount += 1;
                else if (allDiferentMonsters[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Undead)
                    familyCounting.undeadCount += 1;
            }
        }

        //setDesBonus 
        //Demon
        #region
        if (familyCounting.demonCount >= 2)
        {
            if (familyCounting.demonCount == 3)
            {
                for (int i = 0; i < attackers.Count; i++)
                    attackers[i].monsterUpdatedCharacteristics.hitPoint += 25;
            }
            else
                for (int i = 0; i < attackers.Count; i++)
                    attackers[i].monsterUpdatedCharacteristics.hitPoint += 10;
        }
        #endregion

        //Glouton
        #region
        if (familyCounting.gloutonCount >= 2)
        {
            if (familyCounting.gloutonCount >= 4)
            {
                if (familyCounting.gloutonCount >= 6)
                {
                    for (int i = 0; i < attackers.Count; i++)
                    {
                        attackers[i].monsterUpdatedCharacteristics.hitPoint *= 1.35f;
                    }      
                }
                else
                    for (int i = 0; i < attackers.Count; i++)
                    {
                        attackers[i].monsterUpdatedCharacteristics.hitPoint *= 1.2f;
                    }
               
            }
            else
                for (int i = 0; i < attackers.Count; i++)
                {
                    attackers[i].monsterUpdatedCharacteristics.hitPoint *= 1.1f;
                }
        }
        #endregion

        //LightWeight
        #region
        if (familyCounting.lightweightCount >=3)
        {
            /*if (familyCounting.lightweightCount >= 5)
            {
                for (int i = 0; i < attackers.Count; i++)
                {
                    attackers[i].monsterUpdatedCharacteristics.speed *= 1.2f;
                }
            }
            else
                for (int i = 0; i < attackers.Count; i++)
                {
                    for (int y = 0; y < attackers[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies.Length; y++)
                    {
                        if (attackers[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.LightWeight)
                            attackers[i].monsterUpdatedCharacteristics.speed *= 1.2f;
                    }
                }*/
        }
        #endregion

        //Scaled
        if (familyCounting.scaledCount >= 3)
        {
            for (int i = 0; i < attackers.Count; i++)
            {
                attackers[i].monsterUpdatedCharacteristics.defense *= 1.2f;
            }
        }

        //Savages
        #region
        if (familyCounting.savageCount >= 3)
        {
            if (familyCounting.savageCount >= 6)
            {
                for (int i = 0; i < attackers.Count; i++)
                    attackers[i].monsterUpdatedCharacteristics.damageToNexus += 3;
            }
            else
                for (int i = 0; i < attackers.Count; i++)
                {
                    for (int y = 0; y < attackers[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies.Length; y++)
                    {
                        if (attackers[i].monsterCharacteristics.monsterBaseCharacteristics.allEnemyFamilies[y] == FamilyType.Savage)
                            attackers[i].monsterUpdatedCharacteristics.damageToNexus += 2;
                    }
                }
           
                    
        }
        #endregion

        //Undead
        #region
        if (familyCounting.undeadCount >= 2)
        {
            if (familyCounting.undeadCount >= 4)
            {
                for (int i = 0; i < attackers.Count; i++)
                    attackers[i].monsterUpdatedCharacteristics.defense += 0.15f;
            }
            else
                for (int i = 0; i < attackers.Count; i++)
                    attackers[i].monsterUpdatedCharacteristics.defense += 0.08f;
        }
        #endregion
        #endregion

        #region TowerSetup
        TowerTypeCount towerCounting;
        towerCounting.fireCount = 0;
        towerCounting.iceCount = 0;
        towerCounting.rapidFireCount = 0;
        towerCounting.piercingCount = 0;
        towerCounting.heavyCaliberCount = 0;
        towerCounting.multitowerCount = 0;

        List<Mb_Tower> towerTypeList = new List<Mb_Tower>();
        for (int i = 0; i < defenders.Count; i++)
        {
            bool alreadyThere = false;

            for (int y = 0; y < towerTypeList.Count; y++)
            {
                if (defenders[i].towerBaseCharacteristics == towerTypeList[y].towerBaseCharacteristics)
                    alreadyThere = true;
            }

            if (alreadyThere == false)
                towerTypeList.Add(defenders[i]);
        }

        for (int i = 0; i < towerTypeList.Count; i++)
        {
            for (int y = 0; y < towerTypeList[i].towerCharacteristics.allTowerFamily.Length; y++)
            {
                if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.Fire)
                    towerCounting.fireCount += 1;
                else if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.Ice)
                    towerCounting.iceCount += 1;
                else if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.RapidFire)
                    towerCounting.rapidFireCount += 1;
                else if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.Piercing)
                    towerCounting.piercingCount += 1;
                else if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.HeavyCaliber)
                    towerCounting.heavyCaliberCount += 1;
                else if (towerTypeList[i].towerCharacteristics.allTowerFamily[y] == TowerType.MultiTower)
                    towerCounting.multitowerCount += 1;
            }
        }
        
        //setDesBonus 
        //fire
        if (towerCounting.fireCount >=2)
        {
            if (towerCounting.fireCount >= 4)
            {
                if (towerCounting.fireCount >= 6)
                {
                    for (int i = 0; i < defenders.Count; i++)
                        defenders[i].towerCharacteristics.fireDamages += 6;
                }
                else
                    for (int i = 0; i < defenders.Count; i++)
                        defenders[i].towerCharacteristics.fireDamages += 3;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Fire)
                                defenders[i].towerCharacteristics.fireDamages += 3;
        }

        //ice
        if (towerCounting.iceCount >= 2)
        {
            if (towerCounting.iceCount >= 4)
            {
                if (towerCounting.iceCount >= 6)
                {
                    for (int i = 0; i < defenders.Count; i++)
                        defenders[i].towerCharacteristics.slowDuration += 2;
                }
                else
                    for (int i = 0; i < defenders.Count; i++)
                        defenders[i].towerCharacteristics.slowDuration += 1;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Ice)
                            defenders[i].towerCharacteristics.slowDuration += 1;
        }

        //rapidFire
        if (towerCounting.rapidFireCount >= 2)
        {
            if (towerCounting.rapidFireCount >= 4)
            {
                for (int i = 0; i < defenders.Count; i++)
                    defenders[i].towerCharacteristics.delayBetweenAttack *= 0.8f;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.RapidFire)
                            defenders[i].towerCharacteristics.delayBetweenAttack *= 0.8f;
        }

        //Piercing
        if (towerCounting.piercingCount >= 2)
        {
            if (towerCounting.piercingCount >= 4)
            {
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                    if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Piercing)
                        defenders[i].towerCharacteristics.piercingAmount *= 1.5f;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Piercing)
                            defenders[i].towerCharacteristics.piercingAmount *= 1.25f;
        }

        //heavyCaliver
        if (towerCounting.heavyCaliberCount >= 2)
        {
            if (towerCounting.heavyCaliberCount >= 4)
            {
                for (int i = 0; i < defenders.Count; i++)
                    defenders[i].towerCharacteristics.damages += 12;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    defenders[i].towerCharacteristics.damages += 6;
        }

        if (towerCounting.multitowerCount >= 2)
        {
            if (towerCounting.multitowerCount >= 4)
            {
                for (int i = 0; i < defenders.Count; i++)
                            defenders[i].towerCharacteristics.numberOfShots += 1;
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.MultiTower)
                            defenders[i].towerCharacteristics.numberOfShots +=1f;
        }



        #endregion
    }

    [System.Serializable]
    private struct FamilyMonsterCount
        {
        public int demonCount, gloutonCount, lightweightCount, scaledCount, savageCount, undeadCount; 
        }
    [System.Serializable]
    private struct TowerTypeCount
    {
        public int fireCount, iceCount, rapidFireCount, piercingCount, heavyCaliberCount, multitowerCount;
    }
}
