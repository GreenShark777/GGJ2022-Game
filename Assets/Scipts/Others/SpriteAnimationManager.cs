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


    private void Awake()
    {
        //ottiene il numero di sprite presenti nello spritesheet
        nSprites = spriteSheet.Length;

    }
    /// <summary>
    /// Fa partire una nuova animazione, se la priorità di quella da far partire è abbastanza alta
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="nextAnimationIndex"></param>
    /// <param name="lastAnimationIndex"></param>
    public void StartNewAnimation(int priority, int nextAnimationIndex, int lastAnimationIndex)
    {
        //se la priorità di questa animazione è abbastanza alta...
        if (priority >= currentAnimationPriority)
        {
            //...salva la priorità dell'animazione da far partire...
            currentAnimationPriority = priority;
            //...e fa partire l'animazione richiesta
            if (currentAnimationRoutine != null) { StopCoroutine(currentAnimationRoutine); }
            currentAnimationRoutine = StartCoroutine(ManageComboAnimation(nextAnimationIndex, lastAnimationIndex));

        }

    }
    /// <summary>
    /// Si occupa di mostrare l'animazione d'attacco all'indice indicato
    /// </summary>
    /// <param name="nextAnimationIndex">Indice dello sprite a cui andare dello spritesheet d'attacco</param>
    /// <param name="lastAnimationIndex">Indice che indica la fine dell'animazione</param>
    /// <returns></returns>
    private IEnumerator ManageComboAnimation(int nextAnimationIndex, int lastAnimationIndex)
    {
        //aspetta del tempo per rendere l'animazione fluida ma non troppo veloce
        yield return new WaitForSeconds(animationSpeed);
        //se si è arrivati all'ultimo sprite d'animazione, fa terminare l'animazione e fa tornare la priorità a quella minima
        if (nextAnimationIndex > lastAnimationIndex) { currentAnimationPriority = -1; yield break; }
        //cambia lo sprite, continuando l'animazione
        spriteToChange.sprite = spriteSheet[nextAnimationIndex];
        //infine, fa continuare il ciclo d'animazione
        currentAnimationRoutine = StartCoroutine(ManageComboAnimation(nextAnimationIndex + 1, lastAnimationIndex));

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
