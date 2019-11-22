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

    private Phase currentPhase;
    private Coroutine currentPhaseCoroutine;

    private List<Mb_Enemy> attackers;
    private List<Mb_Tower> defenders;

    public PhaseManager()
    {
        currentPhase = Phase.INIT;
    }

    public void Initiate()
    {
        ScanManager.instance.ResetBool();
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

    private IEnumerator InitPhase()
    {
        SetCurrentPhase(Phase.INIT);

        bool isClicked = false;
        while(!isClicked)
        {
            // Verifier si le bouton/ecran a été click 
            isClicked = ScanManager.instance.initValidate;
            yield return 0;
        }
        
        currentPhaseCoroutine = StartCoroutine(DefencePhase());
    }

    private IEnumerator DefencePhase()
    {
        SetCurrentPhase(Phase.DEFENCE);

        defenders.Clear();

        bool isValidate = false;
        while(!isValidate)
        {
            // Check/Scan les cartes
            isValidate = ScanManager.instance.defendersValidate;
            // Ajouter les units correspondante à la liste Defenders
            // + Ajouter/Enlever les units à/de la scène
            yield return 0;
        }

        currentPhaseCoroutine = StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase()
    {
        SetCurrentPhase(Phase.ATTACK);

        attackers.Clear();

        bool isValidate = false;
        while (!isValidate)
        {
            // Check les cartes
            isValidate = ScanManager.instance.attackersValidate;
            // Ajouter/Enlever les units correspondante à la liste Attackers
            // +Ajouter/Enlever les units à/de la scène
            yield return 0;
        }

       currentPhaseCoroutine = StartCoroutine(CompatibilityPhase());
    }

    private IEnumerator CompatibilityPhase()
    {
        SetCurrentPhase(Phase.COMPATIBILITY);

        // Afficher UI
        // Check Compatibility
        // Afficher UI (Bonus) 

        bool isClicked = false;
        while (!isClicked)
        {
            // Verifier si le bouton/ecran a été click
            yield return 0;
        }

        // Desafficher l'UI

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

        bool isClicked = false;
        while(!isClicked)
        {
            // Attendre que le joueur veuille start le prochain round (click bouton/ecran)
            yield return 0;
        }

        // Desafficher l'UI
        
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
            //defender.Action();
        }
    }

    private bool CheckResolutionState()
    {
        foreach(Mb_Enemy enemy in attackers)
        {
            if(enemy.GetUnitState() != UnitState.DEAD)
            {
                return false;
            }
        }

        return true;
    }
}
