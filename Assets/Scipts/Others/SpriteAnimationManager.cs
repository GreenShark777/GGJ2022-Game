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
    //indica quanto velocemente va l'animazione
    [SerializeField]
    private float animationSpeed = 0.1f;
    //indica il numero di sprite presenti nello spritesheet
    private int nSprites;
    //indica la priorità dell'animazione corrente
    private int currentAnimationPriority = -1;
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
        //se l'animazione da fare è automatica, fa partire l'animazione dall'inizio alla fine
        if (automatic) { StartNewAnimation(0, 0, nSprites - 1, false); }

    }
    /// <summary>
    /// Fa partire una nuova animazione, se la priorità di quella da far partire è abbastanza alta
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="nextAnimationIndex"></param>
    /// <param name="lastAnimationIndex"></param>
    public void StartNewAnimation(int priority, int nextAnimationIndex, int lastAnimationIndex, bool realtime = false)
    {
        //se la priorità di questa animazione è abbastanza alta...
        if (priority >= currentAnimationPriority)
        {
            //...salva la priorità dell'animazione da far partire...
            currentAnimationPriority = priority;
            //...e fa partire l'animazione richiesta
            if (currentAnimationRoutine != null) { StopCoroutine(currentAnimationRoutine); }
            currentAnimationRoutine = StartCoroutine(ManageAnimation(nextAnimationIndex, lastAnimationIndex, realtime));

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
            //...fa tornare la priorità a quella minima...
            currentAnimationPriority = -1;
            //...se non è in loop, fa terminare l'animazione...
            if (!isLoop) yield break;
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

}
