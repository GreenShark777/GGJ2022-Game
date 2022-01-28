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
    //indica quanto velocemente va l'animazione
    [SerializeField]
    private float animationSpeed = 0.1f;
    //indica la velocit� impostata inizialmente
    private float startSpeed;
    //indica il numero di sprite presenti nello spritesheet
    private int nSprites;
    //indica la priorit� dell'animazione corrente
    private int currentAnimationPriority = -1;
    //indica l'indice finale dell'animaizione corrente
    private int currentAnimationLastIndex = -1;
    //riferimento alla coroutine che si sta occupando dell'animazione in corso
    private Coroutine currentAnimationRoutine;
    //indica se questo manager parte da solo
    [SerializeField]
    private bool automatic = false;
    //indica se questo manager esegue le animazioni in loop
    [SerializeField]
    private bool isLoop = false;


    private void Awake()
    {
        //ottiene il numero di sprite presenti nello spritesheet
        nSprites = spriteSheet.Length;
        //salva la velocit� impostata inizialmente
        startSpeed = animationSpeed;

    }

    private void OnEnable()
    {
        //se l'animazione da fare � automatica, fa partire l'animazione dall'inizio alla fine
        if (automatic) { StartNewAnimation(0, 0, nSprites - 1, false); }

    }

    /// <summary>
    /// Fa partire una nuova animazione, se la priorit� di quella da far partire � abbastanza alta
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="nextAnimationIndex"></param>
    /// <param name="lastAnimationIndex"></param>
    public void StartNewAnimation(int priority, int nextAnimationIndex, int lastAnimationIndex, bool realtime = false)
    {
        //impedisce di rifare l'animazione corrente, se non � in loop
        if (!isLoop && lastAnimationIndex == currentAnimationLastIndex) { priority = -1; }
        //Debug.Log("Prova a fare animazione");
        //se la priorit� di questa animazione � abbastanza alta...
        if (priority >= currentAnimationPriority)
        {
            //...salva la priorit� e l'indice finale dell'animazione da far partire...
            currentAnimationPriority = priority;
            currentAnimationLastIndex = lastAnimationIndex;
            //...e fa partire l'animazione richiesta
            if (currentAnimationRoutine != null) { StopCoroutine(currentAnimationRoutine); }
            currentAnimationRoutine = StartCoroutine(ManageAnimation(nextAnimationIndex, lastAnimationIndex, realtime));
            //Debug.Log("Sta facendo animazione");
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

        //se si � arrivati all'ultimo sprite d'animazione...
        if (nextAnimationIndex > lastAnimationIndex)
        {
            //...fa tornare la priorit� e l'indice finale ai valori minimi...
            currentAnimationPriority = -1;
            currentAnimationLastIndex = -1;
            //...se non � in loop, fa terminare l'animazione e riporta alla velocit� iniziale...
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
    /// Permette di impostare una nuova velocit�
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetAnimationSpeed(float newSpeed) { animationSpeed = newSpeed; }

}
