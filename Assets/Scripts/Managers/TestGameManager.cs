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
    }

    public int nexusTileID;
    public List<Mb_Enemy> attackers;
    public Coroutine currentCoroutine;
    public System.Random random;
    

    private void Start()
    {
        random = new System.Random();
        currentCoroutine = StartCoroutine(Initialization());
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForFixedUpdate();
        TileManager.instance.InstanciateGrid();
        TileManager.instance.SetPathFinding(nexusTileID);

        for (int i = 0; i < attackers.Count; i++)
        {
            attackers[i].InitializePosition();
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

            yield return new WaitForFixedUpdate();
        }
    }
}
