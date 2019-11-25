using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        GetAllFamilies();
    }

    public int nexusTileID;
    public List<Mb_Enemy> attackers;
    public List<Mb_Tower> defenders;
    public Coroutine currentCoroutine;
    public System.Random random; 

    private void Start()
    {
        random = new System.Random();
        currentCoroutine = StartCoroutine(Initialization());
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

            for (int y = 0; y < allDiferentMonsters.Count; y++)
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

        Debug.Log("demon" + familyCounting.demonCount);
        Debug.Log("gloutonCount" + familyCounting.gloutonCount);
        Debug.Log("lightweightCount" + familyCounting.lightweightCount);
        Debug.Log("scaledCount" + familyCounting.scaledCount);
        Debug.Log("savageCount" + familyCounting.savageCount);
        Debug.Log("undeadCount" + familyCounting.undeadCount);
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
        if (familyCounting.lightweightCount >= 3)
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
        if (towerCounting.fireCount >= 2)
        {
            if (towerCounting.fireCount >= 4)
            {
                if (towerCounting.fireCount >= 6)
                {
                    for (int i = 0; i < defenders.Count; i++)
                    {
                        defenders[i].towerCharacteristics.fireDamages += 6;
                        defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Fire;
                    }
                }
                else
                    for (int i = 0; i < defenders.Count; i++)
                    {
                        defenders[i].towerCharacteristics.fireDamages += 3;
                        defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Fire;
                    }
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Fire)
                        {
                            defenders[i].towerCharacteristics.fireDamages += 3;
                            defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Fire;
                        }
        }

        //ice
        if (towerCounting.iceCount >= 2)
        {
            if (towerCounting.iceCount >= 4)
            {
                if (towerCounting.iceCount >= 6)
                {
                    for (int i = 0; i < defenders.Count; i++)
                    {
                        defenders[i].towerCharacteristics.slowDuration += 2;
                        defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Ice;
                    }
                }
                else
                    for (int i = 0; i < defenders.Count; i++)
                    {
                        defenders[i].towerCharacteristics.slowDuration += 1;
                        defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Ice;
                    }
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Ice)
                        {
                            defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Ice;
                            defenders[i].towerCharacteristics.slowDuration += 1;
                        }
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
                        {
                            defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Piercing;
                            defenders[i].towerCharacteristics.piercingAmount *= 1.5f;
                        }
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                    for (int y = 0; y < defenders[i].towerCharacteristics.allTowerFamily.Length; y++)
                        if (defenders[i].towerCharacteristics.allTowerFamily[y] == TowerType.Piercing)
                        {
                            defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.Piercing;
                            defenders[i].towerCharacteristics.piercingAmount *= 1.25f;
                        }
        }

        //heavyCaliver
        if (towerCounting.heavyCaliberCount >= 2)
        {
            if (towerCounting.heavyCaliberCount >= 4)
            {
                for (int i = 0; i < defenders.Count; i++)
                {
                    defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.HeavyCaliber;
                    defenders[i].towerCharacteristics.damages += 12;
                }
            }
            else
                for (int i = 0; i < defenders.Count; i++)
                {
                    defenders[i].projectileModifierList = defenders[i].projectileModifierList | ProjectileModifier.HeavyCaliber;
                    defenders[i].towerCharacteristics.damages += 6;
                }
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
                            defenders[i].towerCharacteristics.numberOfShots += 1f;
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



    private IEnumerator Initialization()
    {
        yield return new WaitForFixedUpdate();
        TileManager.instance.InstanciateGrid();
        TileManager.instance.SetPathFinding(nexusTileID);

        for (int i = 0; i < attackers.Count; i++)
        {
            attackers[i].Init(0);
        }

        for (int i = 0; i < defenders.Count; i++)
        {
            defenders[i].Init(5);
        }

        currentCoroutine = StartCoroutine(Resolution());
    }

    private IEnumerator Resolution()
    {
        while(true)
        {
            foreach(Mb_Enemy attacker in attackers)
            {
                attacker.Action();
                
            }
            foreach(Mb_Tower defender in defenders)
            {
                defender.Action();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
