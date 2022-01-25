//Si occupa degli attacchi del giocatore
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //riferimento al gameObject con il collider di danno
    [SerializeField]
    private GameObject attackColl = default;
    //riferimento allo sprite da cambiare
    [SerializeField]
    private SpriteRenderer spriteToChange = default;
    //riferimenti a tutti gli sprite di tutti gli attacchi
    [SerializeField]
    private Sprite[] allAttacksSprites = default;
    /*
     * Indica, per l'array di sprites d'attacco, dove ogni attacco inizia e finisce
     * x = inizio animazione
     * y = fine animazione
     */
    [SerializeField]
    private Vector2[] attacksAnimationLimits = default;
    //indica se è già in corso un attacco o meno
    private bool isAttacking = false;

    [SerializeField]
    private float endOfAttack = 0.5f, //indica quanto tempo deve passare tra un attacco e l'altro
        endOfCombo = 0.2f; //indica quanto tempo deve passare dopo un attacco per interrompere la combo

    //indica a che animazione di combo siamo arrivati
    private int comboIndex = -1;
    //indica il numero di combo d'attacco esistenti
    private int nCombos;
    //indica quanto velocemente va l'animazione
    [SerializeField]
    private float animationSpeed = 0.1f;
    //riferimento alla coroutine che si sta occupando delle tempistiche dell'attacco in corso
    private Coroutine currentAttackRoutine;
    //riferimento alla coroutine che si sta occupando dell'animazione dell'attacco in corso
    private Coroutine attackAnimationRoutine;


    private void Awake()
    {
        //disattiva il collider d'attacco all'inizio
        attackColl.SetActive(false);
        //ottiene il numero di combo d'attacco esistenti
        nCombos = attacksAnimationLimits.Length;


        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
        //Corregge e comunica se l'indice finale dell'ultimo limitatore di sprite d'attacco è oltre i limiti
        if (attacksAnimationLimits[nCombos - 1].y >= allAttacksSprites.Length)
        {
            attacksAnimationLimits[nCombos - 1].y = allAttacksSprites.Length - 1;
            Debug.LogError("L'indice finale del limitatore degli sprite d'attacco è oltre il numero di sprite nell'array!");
        }

    }

    public void WantsToAttack()
    {
        //se non è già in corso un attacco...
        if (!isAttacking)
        {
            //...comunica che è in corso un attacco...
            isAttacking = true;
            //...e fa cominciare un nuovo attacco
            if (currentAttackRoutine != null) { StopCoroutine(currentAttackRoutine); }
            currentAttackRoutine = StartCoroutine(ManageAttacksTiming());

        }

    }

    private IEnumerator ManageAttacksTiming()
    {
        //si occupa delle animazioni di combo
        ManageCombos();
        //attiva il collider d'attacco
        attackColl.SetActive(true);
        //aspetta il tempo che finisce l'attacco
        yield return new WaitForSeconds(endOfAttack);
        //disattiva il collider d'attacco
        attackColl.SetActive(false);
        //comunica che non si sta più attaccando
        isAttacking = false;
        //aspetta del tempo per interrompere la combo
        yield return new WaitForSeconds(endOfCombo);
        //la combo viene interrotta
        comboIndex = -1;

    }
    /// <summary>
    /// Funzione che si occupa di mostrare gli sprite giusti per l'attacco della combo attuale
    /// </summary>
    private void ManageCombos()
    {
        //passa alla combo d'attacco successiva
        comboIndex++;
        //se l'indice va oltre i limiti, torna alla prima combo
        if (comboIndex >= nCombos) { comboIndex = 0; }
        //crea nua variabile locale che contiene l'indice iniziale dello spritesheet d'attacco per l'indice della combo
        int animationStartIndex = (int)attacksAnimationLimits[comboIndex].x;
        //infine, fa partire una nuova coroutine per l'animazione d'attacco attuale
        if (attackAnimationRoutine != null) { StopCoroutine(attackAnimationRoutine); }
        attackAnimationRoutine = StartCoroutine(ManageComboAnimation(animationStartIndex, comboIndex));

    }
    /// <summary>
    /// Si occupa di mostrare l'animazione d'attacco all'indice indicato
    /// </summary>
    /// <param name="nextAnimationIndex">Indice dello sprite a cui andare dello spritesheet d'attacco</param>
    /// <param name="attackIndex">Indice dell'attacco di cui si sta facendo l'animazione</param>
    /// <returns></returns>
    private IEnumerator ManageComboAnimation(int nextAnimationIndex, int attackIndex)
    {
        //aspetta del tempo per rendere l'animazione fluida ma non troppo veloce
        yield return new WaitForSeconds(animationSpeed);
        //se lo sprite a cui si vuole andare fa parte dell'animazione di un altro attacco, fa terminare la coroutine
        if (nextAnimationIndex > attacksAnimationLimits[attackIndex].y) { yield break; }
        //cambia lo sprite, continuando l'animazione
        spriteToChange.sprite = allAttacksSprites[nextAnimationIndex];
        //infine, fa continuare il ciclo d'animazione
        attackAnimationRoutine = StartCoroutine(ManageComboAnimation(nextAnimationIndex + 1, attackIndex));

    }

}
