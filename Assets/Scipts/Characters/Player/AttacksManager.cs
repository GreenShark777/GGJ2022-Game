//Si occupa degli attacchi del giocatore
using System.Collections;
using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    //riferimento al gameObject con il collider di danno
    [SerializeField]
    private GameObject attackColl = default;
    //riferimento al manager delle animazioni dello sprite
    [SerializeField]
    private SpriteAnimationManager sam = default;
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
    //riferimento alla coroutine che si sta occupando delle tempistiche dell'attacco in corso
    private Coroutine currentAttackRoutine;


    private void Awake()
    {
        //disattiva il collider d'attacco all'inizio
        attackColl.SetActive(false);
        //ottiene il numero di combo d'attacco esistenti
        nCombos = attacksAnimationLimits.Length;


        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
        //Corregge e comunica se l'indice finale dell'ultimo limitatore di sprite d'attacco è oltre i limiti
        if (attacksAnimationLimits[nCombos - 1].y >= sam.GetNumberOfSprites())
        {
            attacksAnimationLimits[nCombos - 1].y = sam.GetNumberOfSprites() - 1;
            Debug.LogError("L'indice finale del limitatore degli sprite d'attacco è oltre il numero di sprite nell'array!");
        }

    }
    /// <summary>
    /// Esegue l'attacco se non ne è in corso già uno
    /// </summary>
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
    /// <summary>
    /// Si occupa delle tempistiche tra un attacco e l'altro, oltre all'interruzione della combo
    /// </summary>
    /// <returns></returns>
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
        sam.StartNewAnimation(1, animationStartIndex, (int)attacksAnimationLimits[comboIndex].y);

    }
    /// <summary>
    /// Ritorna se un attacco è in corso o meno
    /// </summary>
    /// <returns></returns>
    public bool IsAttacking() { return isAttacking; }

}
