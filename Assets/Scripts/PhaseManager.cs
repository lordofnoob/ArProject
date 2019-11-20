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
    private Phase currentPhase;
    private Coroutine currentPhaseCoroutine;

    private List<GameObject> attackers;
    private List<GameObject> defenders;

    public PhaseManager()
    {
        currentPhase = Phase.INIT;
    }

    public void Initiate()
    {
        currentPhaseCoroutine = StartCoroutine(InitPhase());
    }

    private IEnumerator InitPhase()
    {
        currentPhase = Phase.INIT;

        // Afficher de l'UI (Debuter Round?)

        bool isClicked = false;
        while(!isClicked)
        {
            // Verifier si le bouton/ecran a été click 
            yield return 0;
        }
        
        currentPhaseCoroutine = StartCoroutine(DefencePhase());
    }

    private IEnumerator DefencePhase()
    {
        currentPhase = Phase.DEFENCE;

        // Afficher de L'UI

        defenders.Clear();

        bool isValidate = false;
        while(!isValidate)
        {
            // Check les cartes
            // Ajouter les units correspondante à la liste Defenders
            // + Ajouter les units à la scène
            yield return 0;
        }

        // Desafficher L'UI

        currentPhaseCoroutine = StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase()
    {
        currentPhase = Phase.ATTACK;

        //Afficher de L'UI

        attackers.Clear();

        bool isValidate = false;
        while (!isValidate)
        {
            // Check les cartes
            // Ajouter les units correspondante à la liste Defenders
            // + Ajouter les units à la scène
            yield return 0;
        }
        
        // Desafficher l'UI

        currentPhaseCoroutine = StartCoroutine(CompatibilityPhase());
    }

    private IEnumerator CompatibilityPhase()
    {
        currentPhase = Phase.COMPATIBILITY;

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
        currentPhase = Phase.RESOLUTION;

        // Afficher l'UI?

        bool isResolved = false;
        while(!isResolved)
        {
            // Action des attackers
            // Action des defenders
            yield return 0;
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

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }
}
