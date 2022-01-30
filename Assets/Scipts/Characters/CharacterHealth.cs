using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable, IUpdateData
{
    [Tooltip("Reference to the helth slider used by this character")]
    [SerializeField] private Slider healthSlider;

    [SerializeField]
    private SpriteAnimationManager sam = default;

    [SerializeField]
    private Vector2 takenDmgAnimationLimits = default;
    
    [Range(0, 100)] [SerializeField]
    private int maxHealth;

    private int currentHealth;

    //private UnityAction onDeath;

    //riferimento allo script di vulnerabilità alla pietrificazione, se questa vita riguarda un nemico
    [SerializeField]
    private PetrificationVulnerability pv = default;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //indica se quest'entità ha perso tutta la vita
    private bool lostAllHealth = false;

    [SerializeField] private iScriptableScene tutorial;

    [SerializeField]
    private GameObject objToGetInterfaceFrom = default;

    void Start()
    {
        //imposta la vita corrente a quella salvata, se è i giocatore, altrimenti la imposta alla vita massima
        currentHealth = (!pv) ? g.savedHealth : maxHealth;
        //aggiorna lo slider di vita
        UpdateSlider();

        if (objToGetInterfaceFrom) tutorial = objToGetInterfaceFrom.GetComponent<iScriptableScene>();

        /*
         * ottiene il riferimento alla funzione da cambiare quando finisce la vita
         * se non è un nemico sarà il giocatore, quindi prende come riferimento la funzione per indicare che il giocatore è morto
         * altrimenti, essendo nemico, gli toglie la maschera e lo rende vulnerabile alla pietrificazione
        */
        //onDeath += (!pv) ? PlayerStateManager.PlayerDied : pv.BreakMask;

    }

    private void OnDestroy()
    {

        if (healthSlider) healthSlider.gameObject.SetActive(false);

    }

    /// <summary>
    /// Update the slider value
    /// </summary>
    private void UpdateSlider() { healthSlider.value = (float)currentHealth / (float)maxHealth; }

    // IDamageable   - - -

    /// <summary>
    /// Riceve il valore di danno da sottrarre alla currentHealth
    /// </summary>
    /// <param name="dmgValue">value to be removed from currentHealth</param>
    public void TakeDmg(int dmgValue) {
        // TODO: modifiers on dmg?

        // Evitiamo che il valore della vita esca dai limiti
        currentHealth = Mathf.Clamp(currentHealth - dmgValue, 0, maxHealth);

        // Dopo aver calcolato il nuovo valore, aggiorniamolo a schermo 
        UpdateSlider();

        //se si sta ricevendo danno e non cura, fa partire l'animazione di danno
        if (dmgValue > 0) { StartCoroutine(ShowTakenDmg()); }
        // Se la vita scende a 0 per la prima volta...
        if (currentHealth < 1 && !lostAllHealth)
        {
            //...se è il giocatore, comunica che è morto...
            if (!pv) { PlayerStateManager.SetPlayerDeathState(true); }
            //altrimenti, essendo nemico, gli si rompe la maschera
            else { pv.BreakMask(); }
            //onDeath?.Invoke();
            //...e comunica che si è persa tutta la vita
            lostAllHealth = true;

            // Alla morte del nemico del tutorial, esegui l'animazione
            // P.S: This is really a horror... use actions...
            if (tutorial is null) return;
                tutorial.StartScene();
        }
    }

    private IEnumerator ShowTakenDmg()
    {

        sam.SetSpriteColor(Color.red);

        sam.StartNewAnimation(3, (int)takenDmgAnimationLimits.x, (int)takenDmgAnimationLimits.y);

        yield return new WaitForSeconds(1);

        sam.SetSpriteColor(Color.white);

    }

    //IUpdateData---------------------------------------------------------------------------------------------------------
    public void UpdateData()
    {
        //se è il giocatore, aggiorna la vita rimasta al giocatore
        if (!pv && g) { g.savedHealth = currentHealth; }

    }

    /*
    private void OnDestroy()
    {

        onDeath -= onDeath;

    }*/

}
