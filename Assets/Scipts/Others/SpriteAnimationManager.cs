//Si occupa delle animazioni di uno sprite
using System.Collections;
using UnityEngine;

public class SpriteAnimationManager : MonoBehaviour
{
    //riferimento allo sprite da cambiare
    [SerializeField]
    private SpriteRenderer spriteToChange = default;
    //riferimenti a tutti gli sprite di tutti gli attacchi
    [SerializeField]
    private Sprite[] spriteSheet = default;
    //limiti dell'animazione idle
    [SerializeField]
    private int[] idleAnimationLimits = new int[2];
    //limiti dell'animazione di morte
    [SerializeField]
    private int[] deathAnimationLimits = new int[2];
    //indica quanto velocemente va l'animazione
    [SerializeField]
    private float animationSpeed = 0.1f;
    //indica la velocità impostata inizialmente
    private float startSpeed;
    //indica il numero di sprite presenti nello spritesheet
    private int nSprites;
    //indica la priorità dell'animazione corrente
    private int currentAnimationPriority = -1;
    //indica l'indice finale dell'animaizione corrente
    private int currentAnimationLastIndex = -1;
    //riferimento alla coroutine che si sta occupando dell'animazione in corso
    private Coroutine currentAnimationRoutine;
    //indica se questo manager parte da solo
    [SerializeField]
    private bool automatic = false;

    private bool startAutomatic;
    //indica se questo manager esegue le animazioni in loop
    [SerializeField]
    private bool isLoop = false;
    //indica se è il giocatore
    [SerializeField]
    private bool isPlayer = false;


    private void Awake()
    {
        //ottiene il numero di sprite presenti nello spritesheet
        nSprites = spriteSheet.Length;
        //se non è stata impostata la idle, viene impostata al numero di sprites nello spriteSheet
        if (idleAnimationLimits[1] == 0) { idleAnimationLimits[1] = nSprites - 1; }
        //salva la velocità impostata inizialmente
        startSpeed = animationSpeed;

        startAutomatic = automatic;

    }

    private void OnEnable()
    {

        if (!isPlayer && startAutomatic) 
        {
            automatic = true;
            isLoop = true;
        }

        //se l'animazione da fare è automatica, fa partire l'animazione dall'inizio alla fine
        if (startAutomatic) { StartNewAnimation(0, 0, idleAnimationLimits[1], false); }

    }

    private void OnDisable()
    {
        //se viene disabilitato, non è più automatico
        automatic = false;
        isLoop = false;

    }

    /// <summary>
    /// Fa partire una nuova animazione, se la priorità di quella da far partire è abbastanza alta
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="nextAnimationIndex"></param>
    /// <param name="lastAnimationIndex"></param>
    public void StartNewAnimation(int priority, int nextAnimationIndex, int lastAnimationIndex, bool realtime = false)
    {
        //se lo script è abilitato esegue le animazioni
        if (enabled)
        {
            //se è il giocatore...
            if (isPlayer)
            {
                //...se si è trasformato e la priorità indica che sono solo idle, movimenti e attacchi...
                if (PlayerStateManager.hasTransformed && priority < 100)
                {
                    //...cambia l'animazione in modo che usi invece la versione cattiva
                    int previousStartIndex = nextAnimationIndex;
                    nextAnimationIndex = lastAnimationIndex + 1;
                    lastAnimationIndex += (lastAnimationIndex - previousStartIndex);
                    //Debug.LogWarning("Cambiato in -> " + nextAnimationIndex + " : " + lastAnimationIndex);
                }

            }
            //impedisce di rifare l'animazione corrente, se non è in loop
            if (!isLoop && lastAnimationIndex == currentAnimationLastIndex) { priority = -1; }
            //Debug.Log("Prova a fare animazione");
            //se la priorità di questa animazione è abbastanza alta...
            if (priority >= currentAnimationPriority)
            {
                //...salva la priorità e l'indice finale dell'animazione da far partire...
                currentAnimationPriority = priority;
                currentAnimationLastIndex = lastAnimationIndex;
                //...e fa partire l'animazione richiesta
                if (currentAnimationRoutine != null) { StopCoroutine(currentAnimationRoutine); }
                currentAnimationRoutine = StartCoroutine(ManageAnimation(nextAnimationIndex, lastAnimationIndex, realtime));
                //Debug.Log("Sta facendo animazione");
            }

        }

    }
    /// <summary>
    /// Si occupa di mostrare l'animazione all'indice indicato
    /// </summary>
    /// <param name="nextAnimationIndex">Indice dello sprite a cui andare dello spritesheet d'attacco</param>
    /// <param name="lastAnimationIndex">Indice che indica la fine dell'animazione</param>
    /// <returns></returns>
    private IEnumerator ManageAnimation(int nextAnimationIndex, int lastAnimationIndex, bool realtime)
    {
        //aspetta del tempo per rendere l'animazione fluida ma non troppo veloce
        if (!realtime) { yield return new WaitForSeconds(animationSpeed); }
        else { yield return new WaitForSecondsRealtime(animationSpeed); }

        //se si è arrivati all'ultimo sprite d'animazione...
        if (nextAnimationIndex > lastAnimationIndex)
        {
            //...fa tornare la priorità e l'indice finale ai valori minimi...
            currentAnimationPriority = -1;
            currentAnimationLastIndex = -1;
            //...se non è in loop, fa terminare l'animazione e riporta alla velocità iniziale...
            if (!isLoop) { animationSpeed = startSpeed; yield break; }
            //...altrimenti, la fa ripartire dall'inizio
            else { nextAnimationIndex = 0; }
            
        }
        //cambia lo sprite, continuando l'animazione
        spriteToChange.sprite = spriteSheet[nextAnimationIndex];
        //infine, fa continuare il ciclo d'animazione
        currentAnimationRoutine = StartCoroutine(ManageAnimation(nextAnimationIndex + 1, lastAnimationIndex, realtime));
        
    }
    /// <summary>
    /// Ritorna il numero di sprite nello spritesheet
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfSprites() { return nSprites; }
    /// <summary>
    /// Permette di cambiare lo sprite da cambiare
    /// </summary>
    /// <param name="newSr"></param>
    public void ChangeSpriteToChange(SpriteRenderer newSr) { spriteToChange = newSr; }
    /// <summary>
    /// Riporta all'animazione idle
    /// </summary>
    public void GoBackToIdle() { StartNewAnimation(1, idleAnimationLimits[0], idleAnimationLimits[1]); }
    /// <summary>
    /// Fa partire l'animazione di morte
    /// </summary>
    public void GoToDeath() { StartNewAnimation(1000, deathAnimationLimits[0], deathAnimationLimits[1]); }
    /// <summary>
    /// Permette di impostare una nuova velocità
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetAnimationSpeed(float newSpeed) { animationSpeed = newSpeed; }

    public void SetSpriteColor(Color newColor) { spriteToChange.color = newColor; }

}
